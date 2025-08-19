using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    [SerializeField] PlayerAbilities abilities;
    

    public void UpgradePlayerAbilities()
    {
        foreach (var ability in abilities.GetAbilities())
        {
            try
            {
                if (ability.Upgrade())
                {
                    Debug.Log("Successfully Upgraded " + ability);
                }
                else
                {
                    Debug.Log("Failed to Upgrade " + ability);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    public void ToggleDebugControls(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            if (!Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

        }
    }
}
