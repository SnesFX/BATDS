using System;
using UnityEngine;

[Serializable]
public class KBInputController
{
	public KBCharacterController m_BaseController;

	public bool isRunning;

	public Quaternion currentFacingDirection = Quaternion.Euler(0f, 180f, 0f);

	public void Initialize(KBCharacterController Input)
	{
		m_BaseController = Input;
		DOInit();
	}

	public virtual void DOInit()
	{
	}

	public virtual void DOUpdate()
	{
	}

	public virtual void DOFixedUpdate()
	{
	}

	public virtual void SetIsRunning(bool setRunning)
	{
		if (m_BaseController.m_CanRun && m_BaseController.CharacterAbleToRun)
		{
			isRunning = setRunning;
		}
		else
		{
			isRunning = false;
		}
	}

	public void ForceFacingDirection(Vector3 newDIR)
	{
		currentFacingDirection = Quaternion.LookRotation(newDIR, Vector3.up);
	}

	public virtual void OnDrawGizmos()
	{
	}
}
