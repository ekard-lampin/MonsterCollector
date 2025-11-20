using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    void Awake() { instance = this; }

    public Vector2Int GetMovementInput()
    {
        Vector2Int input = Vector2Int.zero;
        if (Keyboard.current.wKey.isPressed) { input.y++; }
        if (Keyboard.current.sKey.isPressed) { input.y--; }
        if (Keyboard.current.aKey.isPressed) { input.x--; }
        if (Keyboard.current.dKey.isPressed) { input.x++; }

        return input;
    }
}