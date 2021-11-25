using System.Collections;
using System.Collections.Generic;
using TheseusAndMinotaur.Data;
using TheseusAndMinotaur.Data.Deserializer;
using TheseusAndMinotaur.Data.Game;
using TheseusAndMinotaur.Data.Game.PathFinder;
using TheseusAndMinotaur.WorldControllers.Maze;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace TheseusAndMinotaur.WorldControllers
{
    /// <summary>
    ///     Main manager responsible to handle the game
    /// </summary>
    public class WorldGameController : MonoBehaviour
    {
        [SerializeField] private MovementController theseusMovementController;
        [SerializeField] private MovementController minotaurMovementController;
        [SerializeField] private MovementController exitController;
        
        public readonly UnityEvent WrongMovement = new();
        public readonly UnityEvent PathNotFound = new();
        public readonly ShowHintEvent ShowHint = new();
        private BoardGridSpawner _boardGridSpawner;
        private GameLogic _gameLogic;
        private GameState _state;
        public GameStateChangedEvent GameStateChanged = new();

        private InputAction requestedAction;
        public bool HasUndo => _gameLogic.HasUndo;

        public Vector2 BoardWorldSize => _gameLogic.GridSize.ToWorldSize();

        private GameState State
        {
            get => _state;
            set
            {
                if (value == _state) return;
                _state = value;
                GameStateChanged.Invoke(_state);
            }
        }

        private void Awake()
        {
            _boardGridSpawner = FindObjectOfType<BoardGridSpawner>();
        }

        /// <summary>
        ///     Open new Board
        /// </summary>
        /// <param name="boardPath"></param>
        public void OpenBoard(string boardPath)
        {
            var currentBoardConfig = BoardDeserializer.DeserializeFromStreamingAssets(boardPath);
            _gameLogic = new GameLogic(currentBoardConfig);
            _boardGridSpawner.SpawnBoard(currentBoardConfig);
            theseusMovementController.Initialize(currentBoardConfig.TheseusStartPosition);
            minotaurMovementController.Initialize(currentBoardConfig.MinotaurStartPosition);
            exitController.Initialize(currentBoardConfig.Exit);
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
            requestedAction = InputAction.Undo;
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
                requestedAction = InputAction.None;
                while (requestedAction == InputAction.None) yield return null;

                State = GameState.HandleInput;
                var key = requestedAction;

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

                if (movementResult.BoardStatus == BoardStatus.GameOver)
                    State = GameState.GameOver;
                else
                    State = GameState.Active;
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

            requestedAction = inputAction;
        }

        private void HandleHintRequest()
        {
            var (pathFound, directions) = GetHint();
            if (pathFound)
            {
                State = GameState.HandleInput;
                ShowHint.Invoke(_gameLogic.TheseusCurrentPosition, directions);
                State = GameState.ListenUserInput;
            }
            else
            {
                PathNotFound.Invoke();
            }
        }

        public (bool, List<Direction>) GetHint()
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
            if (direction == Direction.None) yield break;
            yield return StartCoroutine(movementController.MoveTo(direction));
        }

        public class GameStateChangedEvent : UnityEvent<GameState>
        {
        }

        /// <summary>
        /// Board found the path and request to show it
        ///   Vector2Int - Theseus start position
        ///   List<Direction> - direction for the path
        /// </summary>
        public class ShowHintEvent : UnityEvent<Vector2Int, List<Direction>>
        {
        }
    }
}