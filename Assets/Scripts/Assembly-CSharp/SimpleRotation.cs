using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
	public Vector3 speed;

	private void Update()
	{
		base.transform.Rotate(speed * Time.deltaTime);
	}
}
