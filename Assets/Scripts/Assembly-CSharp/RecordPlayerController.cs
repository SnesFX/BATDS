using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordPlayerController : MonoBehaviour
{
	private bool isOpen;

	public GameObject DanceUI;

	public RecordPlayers m_RecordPlayers;

	public AudioClip[] trackList;

	private int nextTrack;

	private bool Transitioning;

	private float vol = 1f;

	[Header("UI")]
	public GameObject UIWindow;

	public UINAVObject[] SongSelections;

	public UINAVObject[] DanceSelections;

	private List<int> UnlockedTracks = new List<int>();

	private List<int> UnlockedDance = new List<int>();

	public void Init()
	{
		SetButtonInfo(0, SongSelections[0], SaveManager.DATA.Record_1);
		SetButtonInfo(1, SongSelections[1], SaveManager.DATA.Record_2);
		SetButtonInfo(2, SongSelections[2], SaveManager.DATA.Record_3);
		SetButtonInfo(3, SongSelections[3], SaveManager.DATA.Record_4);
		SetButtonInfo(4, SongSelections[4], SaveManager.DATA.Record_5);
		SetButtonInfo(5, SongSelections[5], SaveManager.DATA.Record_6);
		SetDanceButtonInfo(0, DanceSelections[0], hasDance: true);
		SetDanceButtonInfo(1, DanceSelections[1], SaveManager.DATA.Blueprints >= 1);
		SetDanceButtonInfo(2, DanceSelections[2], SaveManager.DATA.Blueprints >= 2);
		SetDanceButtonInfo(3, DanceSelections[3], SaveManager.DATA.Blueprints >= 3);
		SetDanceButtonInfo(4, DanceSelections[4], SaveManager.DATA.Blueprints >= 4);
		SetDanceButtonInfo(5, DanceSelections[5], SaveManager.DATA.Blueprints >= 5);
		int @int = PlayerPrefs.GetInt("CurrentTrack", 0);
		if (@int != -1)
		{
			m_RecordPlayers.SetClip(trackList[@int]);
		}
		else
		{
			m_RecordPlayers.Stop();
		}
		GameManager.Instance.Player.SetAnimationPropertyHard("DanceNumber", SaveManager.DATA.Dance + 1);
	}

	public void Open()
	{
		GameManager.SubMenu = UIWindow.gameObject;
		isOpen = true;
		UIWindow.gameObject.SetActive(value: true);
		GameManager.Instance.Player.m_MovementLock.Lock(isStatic: true);
		GameManager.Instance.Player.SetAnimationPropertyHard("DanceNumber", SaveManager.DATA.Dance + 1);
	}

	private void SetButtonInfo(int song, UINAVObject button, bool hasSong)
	{
		button.Disabled = !hasSong;
		if (button.Disabled)
		{
			button.GetComponent<Text>().text = "Find tunes to unlock";
		}
		if (hasSong)
		{
			UnlockedTracks.Add(song);
		}
	}

	private void SetDanceButtonInfo(int dance, UINAVObject button, bool hasDance)
	{
		button.Disabled = !hasDance;
		if (button.Disabled)
		{
			button.GetComponent<Text>().text = "Find Dance Blueprints";
		}
		if (hasDance)
		{
			UnlockedDance.Add(dance);
		}
	}

	public void Close()
	{
		if (isOpen)
		{
			UIWindow.gameObject.SetActive(value: false);
			isOpen = false;
			m_RecordPlayers.SetActive(isActive: true);
			GameManager.Instance.Player.m_MovementLock.UnlockStatic();
			DanceUI.SetActive(value: false);
		}
	}

	public void ForceClose()
	{
		UIWindow.gameObject.SetActive(value: false);
		isOpen = false;
		DanceUI.SetActive(value: false);
	}

	public void SetTrack(int track)
	{
		if (track == -1 || UnlockedTracks.Contains(track))
		{
			if (track >= 0 && track < trackList.Length)
			{
				PlayerPrefs.SetInt("CurrentTrack", track);
				PlayerPrefs.Save();
				StartDance();
				nextTrack = track;
				Transitioning = true;
			}
			else if (track == -1)
			{
				PlayerPrefs.SetInt("CurrentTrack", track);
				PlayerPrefs.Save();
				StopDance();
				nextTrack = -1;
				Transitioning = true;
			}
		}
	}

	private void Update()
	{
		if (TDInputManager.Menu == InputButtonState.DOWN || TDInputManager.Run == InputButtonState.DOWN)
		{
			Close();
		}
		if (Transitioning)
		{
			if (vol > 0f)
			{
				vol -= 2f * Time.deltaTime;
			}
			else if (nextTrack != -1)
			{
				m_RecordPlayers.SetClip(trackList[nextTrack]);
				vol = 1f;
				Transitioning = false;
			}
		}
		m_RecordPlayers.SetVolume(vol * (1f - GameManager.Instance.GAME_UI_MANAGER.Fader.m_Graphic.color.a) * vol * 0.9f);
	}

	public void SetDance(int dance)
	{
		if (UnlockedDance.Contains(dance))
		{
			SaveManager.DATA.Dance = dance;
			GameManager.Instance.Player.SetAnimationPropertyHard("DanceNumber", (float)dance + 1f);
			SaveManager.Save();
		}
	}

	public void StopDance()
	{
		GameManager.Instance.Player.SetAnimationPropertyHard("Speed", 10f);
	}

	public void StartDance()
	{
		GameManager.Instance.Player.SetAnimationTrigger("Dance");
	}
}
