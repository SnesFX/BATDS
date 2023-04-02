using UnityEngine;
using UnityEngine.UI;

public class LockMinigame : MonoBehaviour
{
	public static bool isPlaying;

	public RectTransform Dial;

	public RectTransform Marker;

	private bool stopDial;

	private int multiplier;

	private AudioClipPlayer Audio;

	private Interactable_Locker lockerRef;

	public Text ExitButton;

	public Image InteractIcon;

	public Sprite[] InteractSprites;

	private void OnEnable()
	{
		GameManager.SubMenu = base.gameObject;
		if (Audio == null)
		{
			Audio = GetComponent<AudioClipPlayer>();
		}
		Audio.Init();
		isPlaying = true;
		GameManager.Instance.Player.m_MovementLock.Lock(isStatic: true);
		stopDial = false;
		multiplier = Mathf.Clamp(50 * GameManager.Day, 0, 250);
		Marker.rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360));
		if ((bool)Audio)
		{
			Audio.PlayClip(0);
		}
		if (GameManager.ActiveBuildMode == BuildMode.PC)
		{
			ExitButton.gameObject.SetActive(value: false);
		}
		switch (ControlSwapManager.CurrentControlMode)
		{
		case ControlMode.MOBILE:
			InteractIcon.sprite = InteractSprites[0];
			break;
		case ControlMode.KEYBOARD:
			InteractIcon.sprite = InteractSprites[1];
			break;
		case ControlMode.JOYSTICK:
			InteractIcon.sprite = InteractSprites[2];
			break;
		}
	}

	private void Update()
	{
		if (TDInputManager.Menu == InputButtonState.DOWN || TDInputManager.Run == InputButtonState.DOWN)
		{
			Exit();
		}
		if (TDInputManager.Interact == InputButtonState.DOWN)
		{
			Stop();
		}
		if (!stopDial)
		{
			Dial.Rotate(0f, 0f, (150f + (float)multiplier) * Time.deltaTime);
			return;
		}
		if (Dial.localScale.x != 1f)
		{
			Dial.localScale = Vector3.Lerp(Dial.localScale, Vector3.one, 4f * Time.deltaTime);
			Dial.localScale = Vector3.MoveTowards(Dial.localScale, Vector3.one, 1f * Time.deltaTime);
			return;
		}
		float num = Vector3.Angle(Dial.up, Marker.up);
		if (num > -15f && num < 15f)
		{
			Success();
			return;
		}
		stopDial = false;
		if ((bool)Audio)
		{
			Audio.PlayClip(3);
		}
	}

	public void Activate(Interactable_Locker locker)
	{
		lockerRef = locker;
		base.gameObject.SetActive(value: true);
	}

	public void Stop()
	{
		if (!stopDial)
		{
			stopDial = true;
			Dial.localScale = Vector3.one * 1.1f;
			if ((bool)Audio)
			{
				Audio.PlayClip(1);
			}
		}
	}

	public void Success()
	{
		if ((bool)Audio)
		{
			Audio.PlayClip(2);
		}
		base.gameObject.SetActive(value: false);
		ResetPlayer();
		lockerRef.Open();
		isPlaying = false;
	}

	public void Exit()
	{
		base.gameObject.SetActive(value: false);
		if ((bool)Audio)
		{
			Audio.PlayClip(0);
		}
		ResetPlayer();
		isPlaying = false;
	}

	public void ForceClose()
	{
		base.gameObject.SetActive(value: false);
		isPlaying = false;
	}

	private void ResetPlayer()
	{
		GameManager.Instance.Player.m_MovementLock.UnlockStatic();
	}
}
