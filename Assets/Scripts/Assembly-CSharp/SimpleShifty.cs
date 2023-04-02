using UnityEngine;

public class SimpleShifty : MonoBehaviour
{
	public Vector2 speed;

	public Vector2 power;

	private void Start()
	{
	}

	private void Update()
	{
		float x = 0f + Mathf.Sin(Time.time * speed.x) * power.x;
		float y = 0f + Mathf.Sin(Time.time * speed.y) * power.y;
		GetComponent<RectTransform>().localPosition = new Vector2(x, y);
	}
}
