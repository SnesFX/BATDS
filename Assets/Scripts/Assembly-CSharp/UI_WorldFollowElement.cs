using System.Collections;
using UnityEngine;

public class UI_WorldFollowElement : MonoBehaviour
{
	public Transform FollowWorldTarget;

	public Vector3 Offset = new Vector3(0f, 9f, 3f);

	public FollowElementType VisibleBasedOn;

	private RectTransform rec;

	private IEnumerator Start()
	{
		rec = GetComponent<RectTransform>();
		yield return new WaitForEndOfFrame();
		if (!FollowWorldTarget.gameObject.activeInHierarchy)
		{
			base.gameObject.SetActive(value: false);
		}
		else
		{
			base.gameObject.SetActive(CheckVisiblility(VisibleBasedOn));
		}
	}

	public void Deactivate()
	{
		base.gameObject.SetActive(value: false);
	}

	private void Update()
	{
		if ((bool)FollowWorldTarget && FollowWorldTarget.gameObject.activeInHierarchy)
		{
			SetInteractWorldPosition(FollowWorldTarget.transform.position);
		}
	}

	public void SetInteractWorldPosition(Vector3 worldPos)
	{
		Vector2 vector = GameManager.Instance.MainCamRef.WorldToViewportPoint(worldPos + Offset);
		Vector2 anchoredPosition = new Vector2(vector.x * GameManager.Instance.GAME_UI_MANAGER.CanvasRect.sizeDelta.x - GameManager.Instance.GAME_UI_MANAGER.CanvasRect.sizeDelta.x * 0.5f, vector.y * GameManager.Instance.GAME_UI_MANAGER.CanvasRect.sizeDelta.y - GameManager.Instance.GAME_UI_MANAGER.CanvasRect.sizeDelta.y * 0.5f);
		rec.anchoredPosition = anchoredPosition;
	}

	public bool CheckVisiblility(FollowElementType E)
	{
		switch (E)
		{
		case FollowElementType.CHEST:
			if (GameManager.Instance.collectedItems.Count > 0)
			{
				return true;
			}
			return false;
		case FollowElementType.ALICE_KEY:
			if (SaveManager.DATA.Tape_5)
			{
				return true;
			}
			return false;
		case FollowElementType.CANDLES:
			if (SaveManager.DATA.Candles < 4)
			{
				return true;
			}
			return false;
		case FollowElementType.MASK:
			if (SaveManager.DATA.Candles >= 4 && !SaveManager.DATA.FOUND_E)
			{
				return true;
			}
			return false;
		default:
			return true;
		}
	}
}
