using UnityEngine;

public class SelectUIHelper : MonoBehaviour
{
	public bool SelectOnEnable = true;

	public UINAVObject ToSelect;

	public void OnEnable()
	{
		if (SelectOnEnable)
		{
			Select();
		}
	}

	public void Select()
	{
		UINAVManager.Instance.Select(ToSelect);
	}
}
