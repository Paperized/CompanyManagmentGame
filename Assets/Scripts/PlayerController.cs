using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions input;
    private GameManager gameManager;

    private void Start()
    {
        input = new();
        input.Player.Action.performed += Action_performed;
        gameManager = GameManager.Instance;

        input.Enable();
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        if (!gameManager.GameplayStarted)
        {
            gameManager.StartGameplay();
        }
    }
}
