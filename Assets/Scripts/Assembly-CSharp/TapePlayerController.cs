using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapePlayerController : MonoBehaviour
{
	public static bool isOpen;

	public AudioSource Source;

	public AudioClip[] TapeList;

	public Interactable_TapePlayer Interactabe;

	private float vol;

	[Header("UI")]
	public GameObject UIWindow;

	public UINAVObject[] TapeSelections;

	private List<int> UnlockedTapes = new List<int>();

	public void Init()
	{
		SetButtonInfo(0, TapeSelections[0], SaveManager.DATA.Tape_1);
		SetButtonInfo(1, TapeSelections[1], SaveManager.DATA.Tape_2);
		SetButtonInfo(2, TapeSelections[2], SaveManager.DATA.Tape_3);
		SetButtonInfo(3, TapeSelections[3], SaveManager.DATA.Tape_4);
		SetButtonInfo(4, TapeSelections[4], SaveManager.DATA.Tape_5);
		vol = Source.volume;
	}

	public void Open()
	{
		GameManager.SubMenu = UIWindow.gameObject;
		isOpen = true;
		UIWindow.gameObject.SetActive(value: true);
		GameManager.Instance.Player.m_MovementLock.Lock(isStatic: true);
	}

	private void SetButtonInfo(int song, UINAVObject button, bool hasSong)
	{
		button.Disabled = !hasSong;
		if (button.Disabled)
		{
			button.GetComponent<Text>().text = "Tape not found";
		}
		if (hasSong)
		{
			UnlockedTapes.Add(song);
		}
	}

	public void Close()
	{
		if (isOpen)
		{
			UIWindow.gameObject.SetActive(value: false);
			isOpen = false;
			Interactabe.enabled = true;
			GameManager.Instance.Player.m_MovementLock.UnlockStatic();
		}
	}

	public void ForceClose()
	{
		UIWindow.gameObject.SetActive(value: false);
		isOpen = false;
	}

	public void SetTrack(int track)
	{
		if (track == -1 || UnlockedTapes.Contains(track))
		{
			if (track >= 0 && track < TapeList.Length)
			{
				Source.clip = TapeList[track];
				Source.Play();
			}
			else if (track == -1)
			{
				Source.Stop();
			}
		}
	}

	private void Update()
	{
		if (TDInputManager.Menu == InputButtonState.DOWN || TDInputManager.Run == InputButtonState.DOWN)
		{
			Close();
		}
		Source.volume = (1f - GameManager.Instance.GAME_UI_MANAGER.Fader.m_Graphic.color.a) * 1.2f;
	}
}
