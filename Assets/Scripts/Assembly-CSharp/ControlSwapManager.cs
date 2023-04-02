using InControl;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlSwapManager : MonoBehaviour
{
	public static ControlMode CurrentControlMode;

	public InControlInputModule JoystickInput;

	public StandaloneInputModule KeyboardInput;

	private Vector3 lastMousePos;

	private GameObject lastselect;

	private void Start()
	{
		lastMousePos = Input.mousePosition;
		Cursor.visible = false;
		lastselect = EventSystem.current.currentSelectedGameObject;
		ControlMode currentControlMode = CurrentControlMode;
		if (currentControlMode == ControlMode.JOYSTICK)
		{
			SetJoystick();
		}
		else
		{
			SetKeyboard();
		}
	}

	private void Update()
	{
		if ((bool)InputManager.ActiveDevice.AnyButton || (bool)InputManager.ActiveDevice.LeftStick || (bool)InputManager.ActiveDevice.DPad)
		{
			if (GameManager.Instance != null)
			{
				GameManager.Instance.GAME_UI_MANAGER.SetInputUI(isJoystick: true);
			}
			if (Fuseball_Manager.Instance != null)
			{
				Fuseball_Manager.Instance.SetInteractIcon();
			}
			CurrentControlMode = ControlMode.JOYSTICK;
			SetJoystick();
			Cursor.visible = false;
		}
		else if (InputManager.AnyKeyIsPressed)
		{
			if (GameManager.Instance != null)
			{
				GameManager.Instance.GAME_UI_MANAGER.SetInputUI(isJoystick: false);
			}
			if (Fuseball_Manager.Instance != null)
			{
				Fuseball_Manager.Instance.SetInteractIcon();
			}
			SetKeyboard();
			CurrentControlMode = ControlMode.KEYBOARD;
			Cursor.visible = false;
		}
		if (Input.mousePosition != lastMousePos)
		{
			SetKeyboard();
			CurrentControlMode = ControlMode.KEYBOARD;
			Cursor.visible = true;
			lastMousePos = Input.mousePosition;
		}
		if (GameManager.Instance != null && GameManager.ActiveBuildMode == BuildMode.MOBILE && CurrentControlMode != ControlMode.MOBILE)
		{
			CurrentControlMode = ControlMode.MOBILE;
		}
	}

	private void SetJoystick()
	{
		JoystickInput.enabled = true;
		KeyboardInput.enabled = false;
	}

	private void SetKeyboard()
	{
		JoystickInput.enabled = false;
		KeyboardInput.enabled = true;
	}

	private void resetInput()
	{
		if (EventSystem.current.currentSelectedGameObject == null)
		{
			EventSystem.current.SetSelectedGameObject(lastselect);
		}
		else
		{
			lastselect = EventSystem.current.currentSelectedGameObject;
		}
	}
}
