using System;
using UnityEngine;

[Serializable]
public class PlayerController_TopDown : KBInputController
{
	private Vector3 movement;

	private bool CheckedIfDancing;

	private float DanceTimer;

	public override void DOInit()
	{
		base.DOInit();
		m_BaseController.m_CameraController = UnityEngine.Object.FindObjectOfType<CameraController>();
		m_BaseController.m_CameraController.m_FollowCharacter = m_BaseController;
		m_BaseController.m_CameraController.m_Mode = CameraMode.TOP_DOWN;
		m_BaseController.m_CameraController.Init();
	}

	public override void DOUpdate()
	{
		movement = MovementVector();
		if (movement.magnitude > 0.05f)
		{
			currentFacingDirection = Quaternion.LookRotation(movement, Vector3.up);
		}
		if (m_BaseController.SpeedDifference > 0.5f && isRunning)
		{
			GameManager.Instance.DrainBorisStamina();
		}
		if (!CheckedIfDancing)
		{
			if (DanceTimer < 0.8f)
			{
				DanceTimer += Time.deltaTime;
			}
			else
			{
				for (int i = 0; i < GameManager.Instance.GAME_UI_MANAGER.recordPlayer.m_RecordPlayers.Interactables.Length; i++)
				{
					if (PlayerPrefs.GetInt("CurrentTrack", 0) == -1)
					{
						break;
					}
					if (!GameManager.Instance.GAME_UI_MANAGER.recordPlayer.m_RecordPlayers.Interactables[i].isActiveAndEnabled)
					{
						break;
					}
					Vector3 position = GameManager.Instance.GAME_UI_MANAGER.recordPlayer.m_RecordPlayers.Interactables[i].transform.position;
					if (Vector3.Distance(m_BaseController.transform.position, position) < 10f)
					{
						m_BaseController.SetAnimationProperty("Dance", value: true);
					}
				}
				CheckedIfDancing = true;
			}
		}
		if (movement.magnitude > 0.05f)
		{
			CheckedIfDancing = false;
			m_BaseController.SetAnimationProperty("Dance", value: false);
			DanceTimer = 0f;
		}
		if ((bool)m_BaseController.CharacterArt && !m_BaseController.m_MovementLock.IsLocked())
		{
			Quaternion rotation = Quaternion.Lerp(m_BaseController.CharacterArt.rotation, currentFacingDirection, 20f * Time.deltaTime);
			m_BaseController.CharacterArt.rotation = rotation;
		}
		if (GameManager.ActiveBuildMode == BuildMode.PC)
		{
			if (TDInputManager.Run == InputButtonState.DOWN)
			{
				SetIsRunning(setRunning: true);
			}
			else if (TDInputManager.Run != InputButtonState.HELD && isRunning)
			{
				SetIsRunning(setRunning: false);
			}
		}
		else if (TDInputManager.Run == InputButtonState.DOWN)
		{
			SetIsRunning(setRunning: true);
		}
		else if (TDInputManager.Run == InputButtonState.UP && isRunning)
		{
			SetIsRunning(setRunning: false);
		}
		if ((bool)GameManager.Instance.Monster && GameManager.Instance.Monster.isActiveAndEnabled)
		{
			float value = ((Vector3.Distance(m_BaseController.transform.position, GameManager.Instance.Monster.transform.position) < 15f) ? 1f : 0f);
			m_BaseController.SetAnimationProperty("IsNearTarget", value);
		}
		base.DOUpdate();
	}

	public override void DOFixedUpdate()
	{
		m_BaseController.Move(movement);
		base.DOFixedUpdate();
	}

	private Vector3 MovementVector()
	{
		Vector3 zero = Vector3.zero;
		if (Time.timeScale == 0f || m_BaseController.m_MovementLock.IsLocked())
		{
			return zero;
		}
		if (TDInputManager.MoveUp == InputButtonState.HELD)
		{
			zero += Vector3.forward;
		}
		if (TDInputManager.MoveDown == InputButtonState.HELD)
		{
			zero -= Vector3.forward;
		}
		if (TDInputManager.MoveLeft == InputButtonState.HELD)
		{
			zero -= Vector3.right;
		}
		if (TDInputManager.MoveRight == InputButtonState.HELD)
		{
			zero += Vector3.right;
		}
		zero += new Vector3(TDInputManager.LeftStickRaw.x, 0f, TDInputManager.LeftStickRaw.y);
		return zero + new Vector3(m_BaseController.MobileJoystick.Direction.x, 0f, m_BaseController.MobileJoystick.Direction.y);
	}
}
