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

        private void Start()
        {
            _inputController = GetComponent<InputController>();
            _boardGenerator = FindObjectOfType<BoardGenerator>();
            var board = BoardDeserializer.DeserializeFromStreamingAssets("Test/test3.txt");
            _boardGenerator.SpawnBoard(board);
            theseusMovementController.Initialize(Vector2Int.one, board);
            minotaurAI.Initialize(theseusMovementController, board);
            minotaurMovementController = minotaurAI.GetComponent<MovementController>();
            GameLoop();
        }


        private async Task GameLoop()
        {
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
                    // 2.3 Restart
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
                    // 2.4.1 check game over // FIXME: add check here
                }


                // 2.5 move Minotaur
                for (int i = 0; i < GameConfig.Instance.MinotaurStepsPerTurn; i++)
                {

                    var movementResult = await HandleMinotaurMovement();
                    if (movementResult == MovementResultType.NotPossible)
                    {
                        continue;
                    }
                    else
                    {
                        // 2.5.1 check game over
                    }
                }
            } while (true);
        }

        private async Task<MovementResultType> HandleDirectionalInput(InputAction inputAction)
        {
            var direction = inputAction.ToDirection();
            if (theseusMovementController.CanMoveTo(direction))
            {
                await theseusMovementController.MoveTo(direction);
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
                await minotaurMovementController.MoveTo(direction);
                return MovementResultType.Complete;
            }

            return MovementResultType.NotPossible;
        }
        

        private enum MovementResultType
        {
            Complete,
            NotPossible
        }
    }
}