using System;
using UnityEngine;

[Serializable]
public class PlayerController_3rdPerson : KBInputController
{
	private new Quaternion currentFacingDirection = Quaternion.identity;

	public override void DOInit()
	{
		base.DOInit();
		m_BaseController.m_CameraController = UnityEngine.Object.FindObjectOfType<CameraController>();
		m_BaseController.m_CameraController.m_FollowCharacter = m_BaseController;
		m_BaseController.m_CameraController.m_Mode = CameraMode.THIRD_PERSON;
		m_BaseController.m_CameraController.Init();
	}

	public override void DOUpdate()
	{
		Vector3 vector = MovementVector();
		Vector3 vector2 = vector.z * m_BaseController.m_CameraController.transform.forward + vector.x * m_BaseController.m_CameraController.transform.right;
		m_BaseController.Move(vector2);
		if (vector.magnitude > 0f)
		{
			currentFacingDirection = Quaternion.LookRotation(vector2, Vector3.up);
		}
		if ((bool)m_BaseController.CharacterArt)
		{
			Quaternion rotation = Quaternion.Lerp(m_BaseController.CharacterArt.rotation, currentFacingDirection, 20f * Time.deltaTime);
			m_BaseController.CharacterArt.rotation = rotation;
		}
		if (Input.GetKey(KeyCode.Space))
		{
			m_BaseController.Jump();
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			m_BaseController.JumpReset();
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			m_BaseController.m_CameraController.Rotate3rdPerson(1f);
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			m_BaseController.m_CameraController.Rotate3rdPerson(-1f);
		}
		if (m_BaseController.SpeedDifference > 0.5f && isRunning)
		{
			GameManager.Instance.DrainBorisStamina();
		}
		if (TDInputManager.Run == InputButtonState.DOWN)
		{
			SetIsRunning(setRunning: true);
		}
		else if (TDInputManager.Run != InputButtonState.HELD && isRunning)
		{
			SetIsRunning(setRunning: false);
		}
		base.DOUpdate();
	}

	private Vector3 MovementVector()
	{
		Vector3 zero = Vector3.zero;
		if (Input.GetKey(KeyCode.W))
		{
			zero += Vector3.forward;
		}
		if (Input.GetKey(KeyCode.S))
		{
			zero -= Vector3.forward;
		}
		if (Input.GetKey(KeyCode.A))
		{
			zero -= Vector3.right;
		}
		if (Input.GetKey(KeyCode.D))
		{
			zero += Vector3.right;
		}
		return zero;
	}
}
