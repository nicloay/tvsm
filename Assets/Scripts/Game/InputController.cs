using System;
using System.Threading.Tasks;
using UnityEngine;

namespace TheseusAndMinotaur.Game
{
    /// <summary>
    ///     Listen user input defined at <see cref="InputActions" /> with the same name as in project settings in Input section
    /// </summary>
    public class InputController : MonoBehaviour
    {
        private static readonly InputAction[] InputActions = (InputAction[])Enum.GetValues(typeof(InputAction));

        public async Task<InputAction> GetInput()
        {
            do
            {
                foreach (var inputAction in InputActions)
                    if (Input.GetButton(inputAction.ToString()))
                        return inputAction;

                await Task.Yield();
            } while (true);
        }
    }
}