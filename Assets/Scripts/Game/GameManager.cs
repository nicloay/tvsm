using System.Threading.Tasks;
using TheseusAndMinotaur.Data;
using TheseusAndMinotaur.Data.Deserializer;
using TheseusAndMinotaur.Maze;
using UnityEngine;
using UnityEngine.Events;

namespace TheseusAndMinotaur.Game
{
    /// <summary>
    ///     Main manager responsible to handle the game
    /// </summary>
    [RequireComponent(typeof(InputController))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MovementController theseusMovementController;
        [SerializeField] private MinotaurAI minotaurAI;
        [SerializeField] private MovementController minotaurMovementController;

        public readonly UnityEvent WrongMovement = new();
        private BoardGenerator _boardGenerator;
        private InputController _inputController;

        private GameState _state;

        public GameStateChangedEvent GameStateChanged = new();

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


        private void Start()
        {
            _inputController = GetComponent<InputController>();
            _boardGenerator = FindObjectOfType<BoardGenerator>();
            var board = BoardDeserializer.DeserializeFromStreamingAssets("Test/test3.txt");
            _boardGenerator.SpawnBoard(board);
            theseusMovementController.Initialize(Vector2Int.one, board);
            minotaurAI.Initialize(theseusMovementController, board);
            minotaurMovementController = minotaurAI.GetComponent<MovementController>();
            StartGameLoop();
        }

        public void RestartBoard()
        {
            theseusMovementController.ResetToOriginalPosition();
            minotaurMovementController.ResetToOriginalPosition();
            if (State == GameState.GameOver) StartGameLoop();
        }

        private async Task StartGameLoop()
        {
            State = GameState.Active;
            do
            {
                // 1. Listen Input
                var key = await _inputController.GetInput();
                // 2. Handle Input

                if (key == InputAction.Undo)
                {
                    // 2.2 make Undo
                    // do undo here
                }
                else if (key == InputAction.Restart)
                {
                    RestartBoard();
                    await Task.Yield();
                    continue;
                }
                else
                {
                    // 2.4 move Theseus
                    var movementResult = await HandleDirectionalInput(key);
                    if (movementResult == MovementResultType.NotPossible)
                    {
                        WrongMovement.Invoke();
                        continue;
                    }

                    if (HandleGameOver()) break;
                }


                // 2.5 move Minotaur
                for (var i = 0; i < GameConfig.Instance.MinotaurStepsPerTurn; i++)
                {
                    var movementResult = await HandleMinotaurMovement();
                    if (movementResult == MovementResultType.NotPossible) continue;

                    if (HandleGameOver()) break;
                }
            } while (true);
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

        private async Task<MovementResultType> HandleDirectionalInput(InputAction inputAction)
        {
            var direction = inputAction.ToDirection();
            if (theseusMovementController.CanMoveTo(direction))
            {
                State = GameState.ActiveWithMovementOnScreen;
                await theseusMovementController.MoveTo(direction);
                State = GameState.Active;
                return MovementResultType.Complete;
            }

            await Task.Yield();
            return MovementResultType.NotPossible;
        }

        private async Task<MovementResultType> HandleMinotaurMovement()
        {
            var direction = minotaurAI.GetDirectionToTheTarget();
            if (direction != Direction.None)
            {
                State = GameState.ActiveWithMovementOnScreen;
                await minotaurMovementController.MoveTo(direction);
                State = GameState.Active;
                return MovementResultType.Complete;
            }

            return MovementResultType.NotPossible;
        }

        public class GameStateChangedEvent : UnityEvent<GameState>
        {
        }


        private enum MovementResultType
        {
            Complete,
            NotPossible
        }
    }
}