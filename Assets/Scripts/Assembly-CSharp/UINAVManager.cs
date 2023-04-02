using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UINAVManager : MonoBehaviour
{
	public static UINAVManager Instance;

	public List<UINAVObject> NavObjectList = new List<UINAVObject>();

	public UINAVObject currentNavObject;

	public Color SelectedColor;

	public Color DeselectedColor;

	private void Awake()
	{
		Instance = this;
	}

	public void Select(UINAVObject selectThis)
	{
		if (!(selectThis == null))
		{
			if (currentNavObject != null)
			{
				currentNavObject.Deselect();
			}
			currentNavObject = selectThis;
			selectThis.SetSelected();
		}
	}

	public void Update()
	{
		if (!(currentNavObject == null))
		{
			EventSystem.current.SetSelectedGameObject(currentNavObject.UIObject.gameObject);
			if (TDInputManager.MoveUp == InputButtonState.DOWN)
			{
				Select(currentNavObject.Up);
			}
			if (TDInputManager.MoveDown == InputButtonState.DOWN)
			{
				Select(currentNavObject.Down);
			}
			if (TDInputManager.MoveLeft == InputButtonState.DOWN)
			{
				Select(currentNavObject.Left);
			}
			if (TDInputManager.MoveRight == InputButtonState.DOWN)
			{
				Select(currentNavObject.Right);
			}
		}
	}
}
