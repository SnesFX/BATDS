using System.Collections;
using InControl;
using UnityEngine;

public class TDInputManager : MonoBehaviour
{
	public static InputButtonState MoveUp;

	public static InputButtonState MoveLeft;

	public static InputButtonState MoveRight;

	public static InputButtonState MoveDown;

	public static InputButtonState Interact;

	public static InputButtonState Run;

	public static InputButtonState Menu;

	public static Vector2 LeftStickRaw;

	public static bool JoystickUsed;

	public static float MenuDeadzone = 0.3f;

	public static IEnumerator CalculateInputs()
	{
		while (true)
		{
			InputDevice activeDevice = InputManager.ActiveDevice;
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || activeDevice.DPadLeft.IsPressed)
			{
				GetButtonDownState(ref MoveLeft);
			}
			else if (activeDevice.LeftStick.X < 0f - MenuDeadzone && (Time.timeScale == 0f || GameManager.Instance == null || GameManager.SubMenu != null))
			{
				GetButtonDownState(ref MoveLeft);
			}
			else
			{
				GetButtonUpState(ref MoveLeft);
			}
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || activeDevice.DPadRight.IsPressed)
			{
				GetButtonDownState(ref MoveRight);
			}
			else if (activeDevice.LeftStick.X > MenuDeadzone && (Time.timeScale == 0f || GameManager.Instance == null || GameManager.SubMenu != null))
			{
				GetButtonDownState(ref MoveRight);
			}
			else
			{
				GetButtonUpState(ref MoveRight);
			}
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || activeDevice.DPadUp.IsPressed)
			{
				GetButtonDownState(ref MoveUp);
			}
			else if (activeDevice.LeftStick.Y > MenuDeadzone && (Time.timeScale == 0f || GameManager.Instance == null || GameManager.SubMenu != null))
			{
				GetButtonDownState(ref MoveUp);
			}
			else
			{
				GetButtonUpState(ref MoveUp);
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || activeDevice.DPadDown.IsPressed)
			{
				GetButtonDownState(ref MoveDown);
			}
			else if (activeDevice.LeftStick.Y < 0f - MenuDeadzone && (Time.timeScale == 0f || GameManager.Instance == null || GameManager.SubMenu != null))
			{
				GetButtonDownState(ref MoveDown);
			}
			else
			{
				GetButtonUpState(ref MoveDown);
			}
			if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space) || activeDevice.Action1.IsPressed)
			{
				GetButtonDownState(ref Interact);
			}
			else
			{
				GetButtonUpState(ref Interact);
			}
			if (Input.GetKey(KeyCode.Escape) || activeDevice.CommandIsPressed)
			{
				GetButtonDownState(ref Menu);
			}
			else
			{
				GetButtonUpState(ref Menu);
			}
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || activeDevice.Action3.IsPressed || activeDevice.Action2.IsPressed)
			{
				GetButtonDownState(ref Run);
			}
			else
			{
				GetButtonUpState(ref Run);
			}
			LeftStickRaw = activeDevice.LeftStick.Value;
			JoystickUsed = activeDevice.AnyButtonWasPressed;
			yield return new WaitForEndOfFrame();
		}
	}

	private static void GetButtonDownState(ref InputButtonState currentState)
	{
		if (currentState == InputButtonState.INACTIVE)
		{
			currentState = InputButtonState.DOWN;
		}
		else if (currentState == InputButtonState.DOWN)
		{
			currentState = InputButtonState.HELD;
		}
	}

	private static void GetButtonUpState(ref InputButtonState currentState)
	{
		if (currentState == InputButtonState.DOWN || currentState == InputButtonState.HELD)
		{
			currentState = InputButtonState.UP;
		}
		else if (currentState == InputButtonState.UP)
		{
			currentState = InputButtonState.INACTIVE;
		}
	}
}
