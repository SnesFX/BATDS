using UnityEngine;
using UnityEngine.SceneManagement;

public class InitilizeGame : MonoBehaviour
{
	private void Start()
	{
		Screen.sleepTimeout = -1;
		SceneManager.LoadScene(1);
	}
}
