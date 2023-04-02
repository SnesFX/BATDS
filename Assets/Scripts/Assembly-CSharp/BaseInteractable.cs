using UnityEngine;

public class BaseInteractable : MonoBehaviour
{
	private static readonly float interactionRange = 5.5f;

	public bool IsActive = true;

	public float RangeBoost;

	internal AudioClipPlayer Audio;

	private int cooldown;

	public void OnEnable()
	{
		cooldown = 3;
	}

	public virtual void Start()
	{
		Audio = GetComponent<AudioClipPlayer>();
	}

	public virtual void Update()
	{
		if (GameManager.CurrentPauseState == PauseState.UNPAUSING)
		{
			cooldown = 3;
		}
		if (cooldown > 0)
		{
			cooldown--;
		}
		else if (TDInputManager.Interact == InputButtonState.DOWN && Time.timeScale != 0f && !GameManager.Instance.Player.m_MovementLock.IsLocked())
		{
			CheckInteractionSuccess();
		}
		if (CheckDistanceToPlayer())
		{
			GameManager.Instance.GAME_UI_MANAGER.currentInteractable = this;
		}
	}

	public virtual void CheckInteractionSuccess()
	{
		if (IsActive && base.gameObject.activeInHierarchy && base.enabled && CheckDistanceToPlayer())
		{
			DoInteraction();
			cooldown = 3;
		}
	}

	public bool CheckDistanceToPlayer()
	{
		if (!IsActive)
		{
			return false;
		}
		return Vector3.Distance(GameManager.Instance.Player.transform.position, base.transform.position) < interactionRange + RangeBoost;
	}

	private bool MobileTapCheck()
	{
		if (GameManager.ActiveBuildMode == BuildMode.MOBILE)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				if (Input.GetTouch(i).phase == TouchPhase.Began)
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}

	public virtual void DoInteraction()
	{
		if (base.enabled && base.gameObject.activeInHierarchy && (bool)Audio)
		{
			Audio.PlayClip();
		}
	}
}
