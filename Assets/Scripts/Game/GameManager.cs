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
        [SerializeField] private MinotaurAI minotaurAI;
        [SerializeField] private MovementController minotaurMovementController;


        public Vector2 BoardWorldSize => _currentBoard.GetBoardWorldSize();
        public readonly UnityEvent WrongMovement = new();
        private BoardGenerator _boardGenerator;
        private Board _currentBoard;
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
            _currentBoard = BoardDeserializer.DeserializeFromStreamingAssets(boardPath);
            _boardGenerator.SpawnBoard(_currentBoard);
            theseusMovementController.Initialize(_currentBoard.TheseusStartPosition, _currentBoard);
            minotaurAI.Initialize(theseusMovementController, _currentBoard);
            minotaurMovementController = minotaurAI.GetComponent<MovementController>();
            
            StartCoroutine(StartGameLoop());
        }

        public void RestartBoard()
        {
            StopAllCoroutines();
            State = GameState.TerminatingCurrentLoop;
               

            theseusMovementController.ResetToOriginalPosition();
            minotaurMovementController.ResetToOriginalPosition();
            StartCoroutine(StartGameLoop());
        }

        private InputAction requestedAction;

        private IEnumerator StartGameLoop()
        {
            State = GameState.NewGameStarted;
            

            bool terminateMainLoop;
            do
            {
                terminateMainLoop = false;
                
                // Listen Input
                State = GameState.ListenUserInput;
                requestedAction = InputAction.None;
                while (requestedAction == InputAction.None)
                {
                    yield return null;
                }

                var key = requestedAction;
                


                // 2.4 move Theseus
                
                var direction = key.ToDirection();
                if (!theseusMovementController.CanMoveTo(direction))
                {
                    WrongMovement.Invoke();
                    continue;
                }
                yield return StartCoroutine(HandleDirectionalInput(direction));
                
                if (HandleGameOver()) break;

                // 2.5 move Minotaur
                for (var i = 0; i < GameConfig.Instance.MinotaurStepsPerTurn; i++)
                {
                    var minotaurDirection = minotaurAI.GetDirectionToTheTarget();


                    if (minotaurDirection == Direction.None)
                    {
                        break; // break this loop
                    }
                    yield return StartCoroutine(HandleMinotaurMovement(minotaurDirection));


                    if (HandleGameOver())
                    {
                        terminateMainLoop = true;
                        break;  
                    }
                }
            }while (!terminateMainLoop);

            State = GameState.None;
        }

        public void RequestAction(InputAction inputAction)
        {
            if (inputAction == InputAction.Restart)
            {
                RestartBoard();
                return;
            }

            if (inputAction == InputAction.Undo)
            {
                // do undo
                return;
            }


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
            
            State = GameState.ActiveWithMovementOnScreen;
            yield return StartCoroutine(theseusMovementController.MoveTo(direction));
            State = GameState.ListenUserInput;
        }

        private IEnumerator HandleMinotaurMovement(Direction direction)
        {
            State = GameState.ActiveWithMovementOnScreen;
            yield return StartCoroutine(minotaurMovementController.MoveTo(direction));
            State = GameState.ListenUserInput;
        }

        public class GameStateChangedEvent : UnityEvent<GameState>
        {
        }
    }
}