using System.Collections.Generic;
using UnityEngine;

public class LightFlickerEffect : MonoBehaviour
{
	private Light light;

	private Projector projector;

	[Tooltip("Minimum random light intensity")]
	public float minIntensity;

	[Tooltip("Maximum random light intensity")]
	public float maxIntensity = 1f;

	[Tooltip("How much to smooth out the randomness; lower values = sparks, higher = lantern")]
	[Range(1f, 50f)]
	public int smoothing = 5;

	private float StartingfarClipPlane;

	private Queue<float> smoothQueue;

	private float lastSum;

	public void Reset()
	{
		smoothQueue.Clear();
		lastSum = 0f;
	}

	private void Start()
	{
		smoothQueue = new Queue<float>(smoothing);
		if (light == null)
		{
			light = GetComponent<Light>();
		}
		if (projector == null)
		{
			projector = GetComponent<Projector>();
			if (projector != null)
			{
				StartingfarClipPlane = projector.farClipPlane;
			}
		}
	}

	private void Update()
	{
		if (!(light == null) || !(projector == null))
		{
			while (smoothQueue.Count >= smoothing)
			{
				lastSum -= smoothQueue.Dequeue();
			}
			float num = Random.Range(minIntensity, maxIntensity);
			smoothQueue.Enqueue(num);
			lastSum += num;
			if (light != null)
			{
				light.intensity = lastSum / (float)smoothQueue.Count;
			}
			else if (projector != null)
			{
				projector.farClipPlane = StartingfarClipPlane * lastSum / (float)smoothQueue.Count;
			}
		}
	}
}
