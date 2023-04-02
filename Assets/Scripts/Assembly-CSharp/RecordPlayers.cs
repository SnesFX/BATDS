using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RecordPlayers
{
	public AudioSource[] Sources;

	public Interactable_RecordPlayer[] Interactables;

	private List<int> UnlockedDanceMoves = new List<int>();

	public void SetClip(AudioClip track)
	{
		for (int i = 0; i < Sources.Length; i++)
		{
			Sources[i].clip = track;
			Sources[i].Play();
		}
	}

	public void Stop()
	{
		for (int i = 0; i < Sources.Length; i++)
		{
			Sources[i].Stop();
		}
	}

	public void SetVolume(float Volume)
	{
		for (int i = 0; i < Sources.Length; i++)
		{
			Sources[i].volume = Volume;
		}
	}

	public void SetActive(bool isActive)
	{
		for (int i = 0; i < Interactables.Length; i++)
		{
			Interactables[i].IsActive = isActive;
		}
	}
}
