using UnityEngine;

public class SimpleLookAt : MonoBehaviour
{
	public Transform target;

	public Vector3 UpDir = Vector3.up;

	private void LateUpdate()
	{
		base.transform.LookAt(target.position, UpDir);
	}
}
