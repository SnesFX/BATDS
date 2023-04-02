using UnityEngine;

public class DoorwayController : MonoBehaviour
{
	public GameObject ActiveDoorway;

	public GameObject LockedDoorway;

	public GameObject RoomSpawnPoint;

	public GameObject DoorChecker;

	public bool DoorConnected;

	public void CheckForConnectingDoors()
	{
		LockedDoorway.SetActive(value: false);
		if (!DoorConnected)
		{
			Collider[] array = Physics.OverlapSphere(DoorChecker.transform.position, 0.25f, LayerMask.GetMask("Doorway"));
			if (array.Length > 1)
			{
				array[0].transform.parent.gameObject.SetActive(value: false);
				array[1].transform.parent.gameObject.GetComponent<DoorwayController>().DoorConnected = true;
			}
			else
			{
				ActiveDoorway.SetActive(value: false);
				LockedDoorway.SetActive(value: true);
			}
		}
	}
}
