using System.Collections.Generic;
using UnityEngine;

public class UIFlickerEffect : MonoBehaviour
{
	private SpriteRenderer UIIMage;

	[Tooltip("Minimum random light intensity")]
	public float minIntensity;

	[Tooltip("Maximum random light intensity")]
	public float maxIntensity = 1f;

	[Tooltip("How much to smooth out the randomness; lower values = sparks, higher = lantern")]
	[Range(1f, 50f)]
	public int smoothing = 5;

	private Color Col;

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
		if (UIIMage == null)
		{
			UIIMage = GetComponent<SpriteRenderer>();
			if ((bool)UIIMage)
			{
				Col = UIIMage.color;
			}
		}
	}

	private void Update()
	{
		if (!(UIIMage == null))
		{
			while (smoothQueue.Count >= smoothing)
			{
				lastSum -= smoothQueue.Dequeue();
			}
			float num = Random.Range(minIntensity, maxIntensity);
			smoothQueue.Enqueue(num);
			lastSum += num;
			Col.a = lastSum / (float)smoothQueue.Count;
			UIIMage.color = Col;
		}
	}
}
