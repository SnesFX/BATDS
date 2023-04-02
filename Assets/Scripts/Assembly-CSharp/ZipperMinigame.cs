using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZipperMinigame : MonoBehaviour
{
	public static bool isPlaying;

	public RectTransform Node;

	public List<RectTransform> StopPoints;

	private bool stopNode;

	private int multiplier;

	private AudioClipPlayer Audio;

	private Interactable_DeadBoris BorisRef;

	public Text ExitButton;

	public Image InteractIcon;

	public Sprite[] InteractSprites;

	private int StopPointIndex;

	private int PrevIndex;

	private float waitTime;

	private int SuccessCount;

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
		stopNode = false;
		multiplier = Mathf.Clamp(GameManager.Day, 0, 8);
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
		Node.position = StopPoints[StopPointIndex].position;
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
		if (!stopNode)
		{
			if (waitTime <= 0f)
			{
				Node.position = Vector3.MoveTowards(Node.position, StopPoints[StopPointIndex].position, (float)(500 + 50 * multiplier) * Time.deltaTime);
				if (Vector3.Distance(Node.position, StopPoints[StopPointIndex].position) < 1f)
				{
					PrevIndex = StopPointIndex;
					StopPointIndex++;
					if (StopPointIndex > 3)
					{
						StopPoints.Reverse();
						StopPointIndex = 1;
						PrevIndex = 0;
					}
					waitTime = 0.3f - 0.02f * (float)multiplier;
				}
			}
			else
			{
				waitTime -= Time.deltaTime;
			}
		}
		else if (Node.localScale.x != 1f)
		{
			Node.localScale = Vector3.Lerp(Node.localScale, Vector3.one, 4f * Time.deltaTime);
			Node.localScale = Vector3.MoveTowards(Node.localScale, Vector3.one, 1f * Time.deltaTime);
		}
		else
		{
			float num = Vector3.Distance(Node.position, StopPoints[PrevIndex].position);
			float num2 = Vector3.Distance(Node.position, StopPoints[StopPointIndex].position);
			if (num < 25f)
			{
				actuvateDot(PrevIndex);
			}
			else if (num2 < 25f)
			{
				actuvateDot(StopPointIndex);
			}
			else if ((bool)Audio)
			{
				Audio.PlayClip(3);
			}
			stopNode = false;
		}
	}

	private void actuvateDot(int index)
	{
		Image component = StopPoints[index].GetComponent<Image>();
		if (!component.enabled)
		{
			SuccessCount++;
			component.enabled = true;
			if (SuccessCount == 4)
			{
				Success();
			}
			else if ((bool)Audio)
			{
				Audio.PlayClip(1);
			}
		}
	}

	public void Activate(Interactable_DeadBoris Boris)
	{
		BorisRef = Boris;
		base.gameObject.SetActive(value: true);
		SuccessCount = 0;
		foreach (RectTransform stopPoint in StopPoints)
		{
			stopPoint.GetComponent<Image>().enabled = false;
		}
	}

	public void Stop()
	{
		if (!stopNode)
		{
			stopNode = true;
			Node.localScale = Vector3.one * 1.1f;
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
		BorisRef.Open();
		isPlaying = false;
	}

	public void Exit()
	{
		base.gameObject.SetActive(value: false);
		if ((bool)Audio)
		{
			Audio.PlayClip(3);
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
