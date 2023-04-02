using System;
using UnityEngine;

[Serializable]
public class AIController_Monster : BaseAiController
{
	private static int TrackingCount;

	private bool RestartIdleAudio;

	public override void DOInit()
	{
		base.DOInit();
		m_BaseController.m_MovementLock.Lock(isStatic: false, 6f);
		TrackingCount = 0;
	}

	public override void DOUpdate()
	{
		float num = Vector3.Distance(GameManager.Instance.Player.transform.position, m_BaseController.transform.position);
		if (num < 3f && GameManager.Instance.Player.gameObject.activeSelf)
		{
			GameManager.Instance.KillPlayer(m_BaseController);
		}
		if (num < 15f && !m_BaseController.DisableCameraFX)
		{
			float num2 = num / 15f;
			GameManager.Instance.MainCamRef.fieldOfView = 60f - Mathf.Clamp01(1f - num2) * 10f;
			if (GameManager.Instance.GAME_UI_MANAGER.CamShake.ShakePower < 0.4f)
			{
				GameManager.Instance.GAME_UI_MANAGER.CamShake.ShakePower = (1f - num2) * 0.4f;
			}
		}
		else
		{
			GameManager.Instance.MainCamRef.fieldOfView = 60f;
		}
		if (GameManager.Instance.ShoppingListCount == GameManager.Instance.ShoppingListGoal && EnemyTarget == null && GameManager.Instance.Player.isActiveAndEnabled)
		{
			SetEnemyAsTarget(GameManager.Instance.Player.transform, Force: true);
			OnEnemySpotted();
		}
		if (RestartIdleAudio && !m_BaseController.DialogueSource.isPlaying)
		{
			m_BaseController.IdleAudio.Play();
			RestartIdleAudio = false;
		}
		base.DOUpdate();
	}

	public override void OnEnemySpotted()
	{
		base.OnEnemySpotted();
		GameManager.Instance.GAME_UI_MANAGER.CamShake.ShakePower = 1.5f;
		GameManager.Instance.GAME_UI_MANAGER.BendySpotted.SetAlpha(1f);
		GameManager.Instance.GAME_UI_MANAGER.BendySpotted.TransitionOut();
		if (TrackingCount == 0)
		{
			GameManager.Instance.MUSIC_MANAGER.ChangeMusic(m_BaseController.ChaseMusic, 3f);
		}
		TrackingCount++;
		m_BaseController.SetIsRunning(isRunning: true);
		float num = 1f;
		if ((bool)m_BaseController.DialogueSource.clip)
		{
			num = m_BaseController.DialogueSource.time / m_BaseController.DialogueSource.clip.length;
		}
		if (m_BaseController.SpottedPlayerClips.Length != 0 && (!m_BaseController.DialogueSource.isPlaying || num > 0.6f))
		{
			m_BaseController.DialogueSource.clip = m_BaseController.SpottedPlayerClips[UnityEngine.Random.Range(0, m_BaseController.SpottedPlayerClips.Length)];
			m_BaseController.DialogueSource.Play();
		}
		if (m_BaseController.DisableIdleAudioOnChase)
		{
			m_BaseController.IdleAudio.Stop();
		}
	}

	public override void OnEnemyLost()
	{
		TrackingCount--;
		if (TrackingCount == 0)
		{
			GameManager.Instance.MUSIC_MANAGER.ChangeMusic(GameManager.Instance.MUSIC_MANAGER.AmbienceTrack, 0.3f, 0.3f);
		}
		m_BaseController.SetIsRunning(isRunning: false);
		float num = 1f;
		if ((bool)m_BaseController.DialogueSource.clip)
		{
			num = m_BaseController.DialogueSource.time / m_BaseController.DialogueSource.clip.length;
		}
		if (m_BaseController.LostPlayerClips.Length != 0 && (!m_BaseController.DialogueSource.isPlaying || num > 0.3f))
		{
			m_BaseController.DialogueSource.clip = m_BaseController.LostPlayerClips[UnityEngine.Random.Range(0, m_BaseController.LostPlayerClips.Length)];
			m_BaseController.DialogueSource.Play();
		}
		if (m_BaseController.DisableIdleAudioOnChase)
		{
			RestartIdleAudio = true;
		}
	}
}
