using UnityEngine;

public class RoomProperties : MonoBehaviour
{
	public DoorwayController[] Doorways;

	[Header("Object references")]
	public Transform ChunkRefrencePointsParent;

	[ContextMenu("populate Refrence lists")]
	public void PopulateRefrenceLists()
	{
		Doorways = GetComponentsInChildren<DoorwayController>();
		PathfinderNode[] componentsInChildren = base.transform.GetComponentsInChildren<PathfinderNode>();
		foreach (PathfinderNode pathfinderNode in componentsInChildren)
		{
			pathfinderNode.transform.parent.position = new Vector3(Mathf.Round(pathfinderNode.transform.parent.position.x), Mathf.Round(pathfinderNode.transform.parent.position.y), Mathf.Round(pathfinderNode.transform.parent.position.z));
		}
	}
}
