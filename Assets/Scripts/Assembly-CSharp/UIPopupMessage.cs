using UnityEngine;

public class UIPopupMessage : MonoBehaviour
{
	private int TutorialStatus;

	private bool hasFired;

	public bool isTutorialMessage = true;

	public bool DistanceBased = true;

	public bool OnStart;

	public float Delay;

	public string SaveAfterReading = "";

	[Header("Messages")]
	public string PopupMessageKeyboard;

	public string PopupMessageJoystick;

	public string PopupMessageMobile;

	private void Awake()
	{
		if (TutorialStatus == 0)
		{
			TutorialStatus = PlayerPrefs.GetInt("TutorialComplete", 1);
		}
		if (TutorialStatus == 2 && isTutorialMessage)
		{
			base.enabled = false;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (Delay > 0f)
		{
			Delay -= Time.deltaTime;
		}
		else if (OnStart && !hasFired)
		{
			FireMessage();
		}
		else if (DistanceBased && !hasFired && Vector3.Distance(base.transform.position, GameManager.Instance.Player.transform.position) < 10f)
		{
			FireMessage();
		}
	}

	public static void ShowUniqueMessage(string Message)
	{
		GameManager.Instance.GAME_UI_MANAGER.ShowUIPopup(ModifyString(Message));
	}

	public void FireMessage()
	{
		if (SaveAfterReading != "" && PlayerPrefs.GetInt(SaveAfterReading, 0) == 1)
		{
			hasFired = true;
			return;
		}
		string text = ShowMessage();
		if (!(text == ""))
		{
			GameManager.Instance.GAME_UI_MANAGER.ShowUIPopup(text);
			hasFired = true;
			if (SaveAfterReading != "")
			{
				PlayerPrefs.SetInt(SaveAfterReading, 1);
				PlayerPrefs.Save();
			}
		}
	}

	private string ShowMessage()
	{
		switch (ControlSwapManager.CurrentControlMode)
		{
		case ControlMode.MOBILE:
			return ModifyString(PopupMessageMobile);
		case ControlMode.KEYBOARD:
			return ModifyString(PopupMessageKeyboard);
		case ControlMode.JOYSTICK:
			return ModifyString(PopupMessageJoystick);
		default:
			return ModifyString(PopupMessageKeyboard);
		}
	}

	public static string ModifyString(string input)
	{
		return input.Replace("{", "<color=#FFC261FF>").Replace("}", "</color>");
	}
}
