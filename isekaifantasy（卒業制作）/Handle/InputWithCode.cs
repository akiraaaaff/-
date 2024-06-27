using UnityEngine;

public class InputWithCode : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) Joystick.Instance.handle1.OnPointerDown(null);
        if (Input.GetKeyUp(KeyCode.Q)) Joystick.Instance.handle1.OnPointerUp(null);
        if (Input.GetKeyDown(KeyCode.W)) Joystick.Instance.handle2.OnPointerDown(null);
        if (Input.GetKeyUp(KeyCode.W)) Joystick.Instance.handle2.OnPointerUp(null);
        if (Input.GetKeyDown(KeyCode.E)) Joystick.Instance.handle3.OnPointerDown(null);
        if (Input.GetKeyUp(KeyCode.E)) Joystick.Instance.handle3.OnPointerUp(null);
        if (Input.GetKeyDown(KeyCode.R)) Joystick.Instance.handle4.OnPointerDown(null);
        if (Input.GetKeyUp(KeyCode.R)) Joystick.Instance.handle4.OnPointerUp(null);
        if (Input.GetKeyDown(KeyCode.F)) Joystick.Instance.handle5.OnPointerDown(null);
        if (Input.GetKeyUp(KeyCode.F)) Joystick.Instance.handle5.OnPointerUp(null);
    }
}
