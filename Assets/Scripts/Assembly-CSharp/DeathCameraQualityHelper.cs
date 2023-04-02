using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DeathCameraQualityHelper : MonoBehaviour
{
	private void OnEnable()
	{
		Camera.main.renderingPath = OptionsManager.RenderMode;
		Camera.main.GetComponent<PostProcessLayer>().antialiasingMode = OptionsManager.AOMode;
	}
}
