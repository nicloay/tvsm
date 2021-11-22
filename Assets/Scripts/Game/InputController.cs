using System.Threading.Tasks;
using UnityEngine;

namespace TheseusAndMinotaur.Game
{
    /// <summary>
    ///     Listen user input defined at <see cref="DirectionActions" /> with the same name as in project settings in Input
    ///     section
    /// </summary>
    public class InputController : MonoBehaviour
    {
        private static readonly InputAction[] DirectionActions =
            { InputAction.MoveLeft, InputAction.MoveRight, InputAction.MoveUp, InputAction.MoveDown };

        public async Task<InputAction> GetInput()
        {
            do
            {
                if (Input.GetButtonUp(nameof(InputAction.Restart))) return InputAction.Restart;

                if (Input.GetButtonUp(nameof(InputAction.Undo))) return InputAction.Undo;


                foreach (var inputAction in DirectionActions)
                    if (Input.GetButton(inputAction.ToString()))
                        return inputAction;

                await Task.Yield();
            } while (true);
        }
    }
}