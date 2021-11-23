using System.Collections;
using TheseusAndMinotaur.Data;
using TheseusAndMinotaur.Data.Deserializer;
using TheseusAndMinotaur.Maze;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace TheseusAndMinotaur.Game
{
    /// <summary>
    ///     Main manager responsible to handle the game
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MovementController theseusMovementController;
        [SerializeField] private MovementController minotaurMovementController;
        public readonly UnityEvent WrongMovement = new();
        private BoardGenerator _boardGenerator;
        private BoardConfig _currentBoardConfig;
        private BoardGame _boardGame;
        private GameState _state;

        public GameStateChangedEvent GameStateChanged = new();

        private InputAction requestedAction;


        public Vector2 BoardWorldSize => _currentBoardConfig.GetBoardWorldSize();

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
            _boardGenerator = FindObjectOfType<BoardGenerator>();
        }

        private void Start()
        {
            StartBoard("Test/test4.txt");
        }

        private void StartBoard(string boardPath)
        {
            _currentBoardConfig = BoardDeserializer.DeserializeFromStreamingAssets(boardPath);
            _boardGame = new BoardGame(_currentBoardConfig);
            _boardGenerator.SpawnBoard(_currentBoardConfig);
            theseusMovementController.Initialize(_currentBoardConfig.TheseusStartPosition, _currentBoardConfig);
            minotaurMovementController.Initialize(_currentBoardConfig.MinotaurStartPosition, _currentBoardConfig);
            StartCoroutine(StartGameLoop());
        }

        public void RestartBoard()
        {
            StopAllCoroutines();
            _boardGame.Reset();
            State = GameState.TerminatingCurrentLoop;
            
            theseusMovementController.ResetToOriginalPosition();
            minotaurMovementController.ResetToOriginalPosition();
            StartCoroutine(StartGameLoop());
        }

        private IEnumerator StartGameLoop()
        {
            State = GameState.NewGameStarted;
            do
            {
                // Listen Input
                State = GameState.ListenUserInput;
                requestedAction = InputAction.None;
                while (requestedAction == InputAction.None) yield return null;

                State = GameState.HandleInput;
                var key = requestedAction;

                var direction = key.ToDirection();
                if (!_boardGame.IsMoveAvailableForTheseus(direction))
                {
                    WrongMovement.Invoke();
                    continue;
                }

                var movementResult = _boardGame.MakeMovement(direction);

                if (movementResult.TheseusMove != Direction.None)
                {
                    yield return StartCoroutine(HandleDirectionalInput(movementResult.TheseusMove));
                }

                for (int i = 0; i < 2; i++)
                {
                    if (movementResult.Moves[i + 1] != Direction.None)
                    {
                        yield return StartCoroutine(HandleMinotaurMovement(movementResult.Moves[i + 1]));
                    }
                }

                if (movementResult.BoardStatus == BoardStatus.Victory)
                {
                    State = GameState.Victory;
                    break;
                }
                else if (movementResult.BoardStatus == BoardStatus.GameOver)
                {
                    State = GameState.GameOver;
                }
                else
                {
                    State = GameState.Active;
                }
            } while (State == GameState.Active);
        }

        public void RequestAction(InputAction inputAction)
        {
            if (inputAction == InputAction.Restart)
            {
                RestartBoard();
                return;
            }

            if (inputAction == InputAction.Undo)
                // do undo
                return;


            if (State != GameState.ListenUserInput)
            {
                Debug.LogError($"you can only request input action in {GameState.ListenUserInput} state");
                return;
            }

            requestedAction = inputAction;
        }

        /// <summary>
        ///     Check if Minotaur caught Theseus.
        ///     If yes - raise Event
        /// </summary>
        /// <returns>return true if Minotaur caught Theseus, false otherwise</returns>
        private bool HandleGameOver()
        {
            var result = minotaurMovementController.CurrentBoardPosition ==
                         theseusMovementController.CurrentBoardPosition;
            if (result) State = GameState.GameOver;

            return result;
        }

        private IEnumerator HandleDirectionalInput(Direction direction)
        {
            Assert.IsTrue(theseusMovementController.CanMoveTo(direction));

            State = GameState.Active;
            yield return StartCoroutine(theseusMovementController.MoveTo(direction));
            State = GameState.ListenUserInput;
        }

        private IEnumerator HandleMinotaurMovement(Direction direction)
        {
            State = GameState.Active;
            yield return StartCoroutine(minotaurMovementController.MoveTo(direction));
            State = GameState.ListenUserInput;
        }

        public class GameStateChangedEvent : UnityEvent<GameState>
        {
        }
    }
}