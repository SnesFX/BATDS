using UnityEngine;

public class Fuseball_Pattle : MonoBehaviour
{
	public float PattleSpeed;

	private int MoveDir;

	private void Start()
	{
		StartCoroutine(TDInputManager.CalculateInputs());
	}

	private void Update()
	{
		if (Fuseball_Manager.Instance.GameStarted)
		{
			if (TDInputManager.MoveLeft == InputButtonState.HELD || MoveDir < 0)
			{
				base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, new Vector3(-1.5f, -0.7f, -1f), PattleSpeed * Time.deltaTime);
			}
			if (TDInputManager.MoveRight == InputButtonState.HELD || MoveDir > 0)
			{
				base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, new Vector3(1.4f, -0.7f, -1f), PattleSpeed * Time.deltaTime);
			}
		}
	}

	public void MovePattleLeft()
	{
		MoveDir = -1;
	}

	public void MovePattleRight()
	{
		MoveDir = 1;
	}

	public void StopMovement()
	{
		MoveDir = 0;
	}
}
