using System.Threading.Tasks;
using UnityEngine;

namespace TheseusAndMinotaur.Game
{
    public class InputController : MonoBehaviour
    {
        public async Task<InputAction> GetInput()
        {
            do
            {
                if (Input.GetButton(nameof(InputAction.MoveDown)))
                {
                    return InputAction.MoveDown;
                }
                else if (Input.GetButton(nameof(InputAction.MoveLeft)))
                {
                    return InputAction.MoveLeft;
                }
                else if (Input.GetButton(nameof(InputAction.MoveRight)))
                {
                    return InputAction.MoveRight;
                }
                else if (Input.GetButton(nameof(InputAction.MoveUp)))
                {
                    return InputAction.MoveUp;
                }
                else if (Input.GetButtonDown(nameof(InputAction.Undo)))
                {
                    return InputAction.Undo;
                }
                else if (Input.GetButtonDown(nameof(InputAction.Restart)))
                {
                    return InputAction.Restart;
                }

                await Task.Yield();
            } while (true);
        }
    }
}