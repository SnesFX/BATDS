using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
	private static CoroutineRunner instance;

	public static CoroutineRunner Instance
	{
		get
		{
			if (!instance)
			{
				instance = Object.FindObjectOfType<CoroutineRunner>();
			}
			return instance;
		}
	}
}
