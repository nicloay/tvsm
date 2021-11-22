using System;
using System.Threading.Tasks;
using TheseusAndMinotaur.Data;
using TheseusAndMinotaur.Data.Deserializer;
using TheseusAndMinotaur.Maze;
using UnityEngine;
using UnityEngine.Events;

namespace TheseusAndMinotaur.Game
{
    
    /// <summary>
    /// Main manager responsible to handle the game
    /// </summary>
    [RequireComponent(typeof(InputController))]
    public class GameManager : MonoBehaviour
    {
        public readonly UnityEvent WrongMovement = new UnityEvent();
        [SerializeField] private MovementController _theseusMovementController;
        private InputController _inputController;
        private BoardGenerator _boardGenerator;
        private void Start()
        {
            _inputController = GetComponent<InputController>();
            _boardGenerator = FindObjectOfType<BoardGenerator>();
            var board = BoardDeserializer.DeserializeFromStreamingAssets("Test/test3.txt");
            _boardGenerator.SpawnBoard(board);
            _theseusMovementController.Initialize(Vector2Int.one, board);
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
                        continue; //nothing changed on board - repeat loop from beginning
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


        private enum InputResult
        {
            Complete,
            WrongAction
        }
        
        private async Task<InputResult> HandleDirectionalInput(InputAction inputAction)
        {
            var direction = inputAction.ToDirection();
            if (_theseusMovementController.CanMoveTo(direction))
            {
                await _theseusMovementController.MoveTo(direction);
                return InputResult.Complete; 
            }
            await Task.Yield(); 
            return InputResult.WrongAction;
        }
    }
}