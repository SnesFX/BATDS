using UnityEngine;

public class LoadingController : MonoBehaviour
{
	public static UITransitionHelper T;

	public void Start()
	{
		Object.DontDestroyOnLoad(base.transform.parent.gameObject);
		T = GetComponent<UITransitionHelper>();
	}

	public static void IsLoading()
	{
		if ((bool)T)
		{
			T.TransitionIn();
		}
	}

	public static void LoadingComplete()
	{
		if ((bool)T)
		{
			T.TransitionOut();
		}
	}
}
