using System;
using UnityEngine;

[Serializable]
public class PlayerController_1stPerson : KBInputController
{
	public override void DOInit()
	{
		base.DOInit();
		m_BaseController.m_CameraController = UnityEngine.Object.FindObjectOfType<CameraController>();
		m_BaseController.m_CameraController.m_FollowCharacter = m_BaseController;
		m_BaseController.m_CameraController.m_Mode = CameraMode.FIRST_PERSON;
		m_BaseController.m_CameraController.Init();
	}

	public override void DOUpdate()
	{
		Vector3 vector = MovementVector();
		Vector3 direction = vector.z * m_BaseController.m_CameraController.transform.forward + vector.x * m_BaseController.m_CameraController.transform.right;
		m_BaseController.Move(direction);
		if (Input.GetKey(KeyCode.Space))
		{
			m_BaseController.Jump();
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			m_BaseController.JumpReset();
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			m_BaseController.JumpReset();
		}
		base.DOUpdate();
	}

	public override void DOFixedUpdate()
	{
		base.DOFixedUpdate();
		m_BaseController.transform.rotation = m_BaseController.m_CameraController.transform.rotation;
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
