using System.Threading.Tasks;
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
                    var movementResult = await HandleDirectionalInput(key);
                    if (movementResult == InputResult.WrongAction)
                    {
                        WrongMovement.Invoke();
                        continue;
                    }
                }

                // 2.4 move Theseus

                // 2.4.1 check game over

                // 2.5 move Minotaur

                // 2.5.1 check game over

                // 2.6 move Minotaur

                // 2.6.1 check game over
            } while (true);
        }

        private async Task<InputResult> HandleDirectionalInput(InputAction inputAction)
        {
            var direction = inputAction.ToDirection();
            if (theseusMovementController.CanMoveTo(direction))
            {
                await theseusMovementController.MoveTo(direction);
                return InputResult.Complete;
            }

            await Task.Yield();
            return InputResult.WrongAction;
        }


        private enum InputResult
        {
            Complete,
            WrongAction
        }
    }
}