using UnityEngine;

public class SimplePulse : MonoBehaviour
{
	public float speed;

	public float power;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.localScale = Vector3.one + Vector3.one * Mathf.Sin(Time.time * speed) * power;
	}
}
