using System.Collections;
using System.Collections.Generic;
using TheseusAndMinotaur.Data;
using TheseusAndMinotaur.Data.Converter;
using TheseusAndMinotaur.Data.Game;
using TheseusAndMinotaur.Data.Game.PathFinder;
using TheseusAndMinotaur.Visual.World;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace TheseusAndMinotaur.WorldControllers
{
    /// <summary>
    ///     Main manager responsible to handle the game
    /// </summary>
    public class WorldGameManager : MonoBehaviour
    {
        [SerializeField] private MovementController theseusMovementController;
        [SerializeField] private MovementController minotaurMovementController;
        [SerializeField] private MovementController exitController;
        public readonly GameStateChangedEvent GameStateChanged = new();
        public readonly LevelStartEvent LevelStart = new();
        public readonly LoadLevelCommunicationProblemEvent LoadLevelCommunicationProblem = new();
        public readonly UnityEvent PathNotFound = new();
        public readonly ShowHintEvent ShowHint = new();
        public readonly UnityEvent WrongMovement = new();
        private GameLogic _gameLogic;

        private InputAction _requestedAction;
        private GameState _state;
        private WorldGridManager _worldGridManager;
        public bool HasUndo => _gameLogic.HasUndo;

        public Vector2 BoardWorldSize => _gameLogic.GridSize.ToWorldSize();

        private GameState State
        {
            get => _state;
            set
            {
                if (value == _state)
                {
                    return;
                }

                _state = value;
                GameStateChanged.Invoke(_state);
            }
        }

        private void Awake()
        {
            _worldGridManager = FindObjectOfType<WorldGridManager>();
        }

        /// <summary>
        ///     Open new Board
        /// </summary>
        /// <param name="boardResourcePath">path to the board TEXT asset in Resources folder</param>
        public void OpenBoard(string boardResourcePath)
        {
            var fullPath = boardResourcePath.GetResourceContent();
            StartBoard(fullPath, boardResourcePath.GetLevelName());
        }

        public void StartBoard(string textConfig, string levelName)
        {
            var currentBoardConfig = textConfig.ToBoardConfig();
            _gameLogic = new GameLogic(currentBoardConfig);
            _worldGridManager.SpawnBoard(currentBoardConfig);
            theseusMovementController.Initialize(currentBoardConfig.TheseusStartPosition);
            minotaurMovementController.Initialize(currentBoardConfig.MinotaurStartPosition);
            exitController.Initialize(currentBoardConfig.Exit);
            LevelStart.Invoke(levelName);
            StartNewGame();
        }

        public void RestartBoard()
        {
            StopAllCoroutines();
            _gameLogic.Reset();
            State = GameState.TerminatingCurrentLoop;

            theseusMovementController.ResetToOriginalPosition();
            minotaurMovementController.ResetToOriginalPosition();
            StartNewGame();
        }

        public void RequestUndoOnFinishedGame()
        {
            StartCoroutine(StartGameLoop());
            _requestedAction = InputAction.Undo;
        }


        private void StartNewGame()
        {
            State = GameState.NewGameStarted;
            StartCoroutine(StartGameLoop());
        }

        private IEnumerator StartGameLoop()
        {
            do
            {
                // Listen Input
                State = GameState.ListenUserInput;
                _requestedAction = InputAction.None;
                while (_requestedAction == InputAction.None)
                {
                    yield return null;
                }

                State = GameState.HandleInput;
                var key = _requestedAction;

                if (key == InputAction.Undo)
                {
                    Assert.IsTrue(_gameLogic.HasUndo);
                    var undoResult = _gameLogic.Undo();
                    yield return StartCoroutine(MoveCharacters(undoResult));
                    State = GameState.Active;
                    continue;
                }

                var direction = key.ToDirection();
                if (!_gameLogic.IsMoveAvailableForTheseus(direction))
                {
                    WrongMovement.Invoke();
                    State = GameState.Active;
                    continue;
                }

                var movementResult = _gameLogic.MakeMovement(direction);
                if (!movementResult.BoardChanged)
                {
                    State = GameState.Active;
                    continue;
                }

                yield return StartCoroutine(MoveCharacters(movementResult));

                if (movementResult.BoardStatus == BoardStatus.Victory)
                {
                    State = GameState.Victory;
                    break;
                }

                State = movementResult.BoardStatus == BoardStatus.GameOver
                    ? GameState.GameOver
                    : GameState.Active;
            } while (State == GameState.Active);
        }

        public void RequestAction(InputAction inputAction)
        {
            switch (inputAction)
            {
                case InputAction.Restart:
                    RestartBoard();
                    return;
                case InputAction.Hint:
                {
                    HandleHintRequest();
                    return;
                }
            }

            if (State != GameState.ListenUserInput)
            {
                Debug.LogError($"you can only request input action in {GameState.ListenUserInput} state");
                return;
            }

            _requestedAction = inputAction;
        }

        private void HandleHintRequest()
        {
            var (pathFound, directions) = GetHint();
            if (pathFound == PathFinder.Result.PathNotFound)
            {
                PathNotFound.Invoke();
            }
            else
            {
                State = GameState.HandleInput;
                ShowHint.Invoke(_gameLogic.TheseusCurrentPosition, directions);
                State = GameState.ListenUserInput;
            }
        }

        private (PathFinder.Result, List<Direction>) GetHint()
        {
            var pathFinder = new PathFinder(_gameLogic);
            return pathFinder.FindPath();
        }

        private IEnumerator MoveCharacters(BoardMoveResult moveResult)
        {
            yield return StartCoroutine(MoveCharacter(theseusMovementController, moveResult.TheseusMove));
            yield return StartCoroutine(MoveCharacter(minotaurMovementController, moveResult.MinotaurFirstMove));
            yield return StartCoroutine(MoveCharacter(minotaurMovementController, moveResult.MinotaurSecondMove));
        }

        private IEnumerator MoveCharacter(MovementController movementController, Direction direction)
        {
            if (direction == Direction.None)
            {
                yield break;
            }

            yield return StartCoroutine(movementController.MoveTo(direction));
        }

        public class GameStateChangedEvent : UnityEvent<GameState>
        {
        }

        /// <summary>
        ///     Board found the path and request to show it
        ///     Vector2Int - Theseus start position
        ///     List{Direction} - direction for the path
        /// </summary>
        public class ShowHintEvent : UnityEvent<Vector2Int, List<Direction>>
        {
        }

        /// <summary>
        ///     Event raised when new board started
        ///     Provide info of the level name which is equal FileNameWithoutExtension
        /// </summary>
        public class LevelStartEvent : UnityEvent<string>
        {
        }


        public class LoadLevelCommunicationProblemEvent : UnityEvent<string>
        {
        }
    }
}