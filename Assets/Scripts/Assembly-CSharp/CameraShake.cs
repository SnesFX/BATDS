using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public float ShakePower;

	private Vector3 TargetPos;

	private void Start()
	{
		TargetPos = new Vector3(0f, 0f, base.transform.localPosition.z);
	}

	private void Update()
	{
		if (ShakePower > 0f)
		{
			ShakePower -= Time.deltaTime;
			TargetPos = new Vector3(Random.Range(-1f, 1f) * ShakePower, Random.Range(-1f, 1f) * ShakePower, base.transform.localPosition.z);
		}
		else if (TargetPos.x != 0f || TargetPos.y != 0f)
		{
			TargetPos = new Vector3(0f, 0f, base.transform.localPosition.z);
		}
		base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, TargetPos, 10f * Time.deltaTime);
	}
}
