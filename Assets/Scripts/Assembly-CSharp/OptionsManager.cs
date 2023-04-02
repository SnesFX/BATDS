using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
	public BuildMode ActiveBuildMode;

	public Toggle m_FullScreen;

	public SliderDisplay s_Resolution;

	public SliderDisplay s_Quality;

	public Toggle m_VSync;

	public SliderDisplay s_Music;

	public SliderDisplay s_Audio;

	public MaterialSwapper[] MaterialSwap;

	public static RenderingPath RenderMode;

	public static PostProcessLayer.Antialiasing AOMode;

	private List<Resolution> Resolutions = new List<Resolution>();

	private void PullBuildMode()
	{
		ActiveBuildMode = BuildMode.PC;
	}

	private void OnEnable()
	{
		PullBuildMode();
		if (ActiveBuildMode == BuildMode.MOBILE)
		{
			m_FullScreen.transform.parent.gameObject.gameObject.SetActive(value: false);
			m_VSync.transform.parent.gameObject.gameObject.SetActive(value: false);
			s_Resolution.m_Slider.transform.parent.gameObject.SetActive(value: false);
			s_Quality.m_Slider.gameObject.GetComponent<UINAVObject>().Select();
		}
		else
		{
			m_FullScreen.isOn = Screen.fullScreen;
			m_VSync.isOn = PlayerPrefs.GetInt("VSYNC", 1) == 1;
			Resolutions.Clear();
			Resolutions.AddRange(Screen.resolutions);
			s_Resolution.m_Slider.maxValue = Resolutions.Count - 1;
			s_Resolution.m_Slider.value = PlayerPrefs.GetInt("Resolution", Resolutions.Count - 1);
			UpdateResolution();
			m_FullScreen.gameObject.GetComponent<UINAVObject>().Select();
		}
		s_Quality.m_Slider.value = PlayerPrefs.GetInt("Quality", 2);
		UpdateQuality();
		SetQuality();
		UpdateAudio();
	}

	public void Update()
	{
	}

	public void UpdateQuality()
	{
		switch ((int)s_Quality.m_Slider.value)
		{
		case 0:
			s_Quality.m_Value.text = "Low";
			break;
		case 1:
			s_Quality.m_Value.text = "Medium";
			break;
		case 2:
			s_Quality.m_Value.text = "High";
			break;
		default:
			s_Quality.m_Value.text = "ERR";
			break;
		}
		PlayerPrefs.SetInt("Quality", (int)s_Quality.m_Slider.value);
	}

	public void UpdateResolution()
	{
		if (Resolutions.Count > 0)
		{
			Resolution resolution = Resolutions[(int)s_Resolution.m_Slider.value];
			PlayerPrefs.SetInt("Resolution", (int)s_Resolution.m_Slider.value);
			s_Resolution.m_Value.text = resolution.width + "X" + resolution.height;
		}
	}

	public void UpdateAudio()
	{
		s_Music.m_Slider.value = PlayerPrefs.GetFloat("Music", 1f);
		s_Music.m_Value.text = Mathf.Round(s_Music.m_Slider.value * 100f) + "%";
		s_Audio.m_Slider.value = PlayerPrefs.GetFloat("Sound", 1f);
		s_Audio.m_Value.text = Mathf.Round(s_Audio.m_Slider.value * 100f) + "%";
	}

	public void SetQuality()
	{
		Object.FindObjectOfType<PostProcessVolume>();
		switch (PlayerPrefs.GetInt("Quality", 2))
		{
		case 0:
			SetQualityLow();
			break;
		case 1:
			SetQualityMed();
			break;
		case 2:
			SetQualityHigh();
			break;
		}
	}

	public void SetQualityLow()
	{
		PostProcessVolume postProcessVolume = Object.FindObjectOfType<PostProcessVolume>();
		if (ActiveBuildMode == BuildMode.MOBILE)
		{
			RenderMode = RenderingPath.VertexLit;
			QualitySettings.shadowResolution = ShadowResolution.Low;
			QualitySettings.masterTextureLimit = 3;
			if ((bool)postProcessVolume)
			{
				if (postProcessVolume.sharedProfile.TryGetSettings<Bloom>(out var outSetting))
				{
					outSetting.active = false;
				}
				if (postProcessVolume.sharedProfile.TryGetSettings<ColorGrading>(out var outSetting2))
				{
					outSetting2.active = false;
				}
			}
			return;
		}
		AOMode = PostProcessLayer.Antialiasing.None;
		RenderMode = RenderingPath.DeferredShading;
		QualitySettings.shadowResolution = ShadowResolution.Medium;
		if ((bool)postProcessVolume)
		{
			if (postProcessVolume.sharedProfile.TryGetSettings<AmbientOcclusion>(out var outSetting3))
			{
				outSetting3.active = false;
			}
			if (postProcessVolume.sharedProfile.TryGetSettings<MotionBlur>(out var outSetting4))
			{
				outSetting4.active = false;
			}
		}
	}

	public void SetQualityMed()
	{
		PostProcessVolume postProcessVolume = Object.FindObjectOfType<PostProcessVolume>();
		if (ActiveBuildMode == BuildMode.MOBILE)
		{
			RenderMode = RenderingPath.Forward;
			QualitySettings.shadowResolution = ShadowResolution.Low;
			QualitySettings.masterTextureLimit = 3;
			if ((bool)postProcessVolume)
			{
				if (postProcessVolume.sharedProfile.TryGetSettings<Bloom>(out var outSetting))
				{
					outSetting.active = true;
				}
				if (postProcessVolume.sharedProfile.TryGetSettings<ColorGrading>(out var outSetting2))
				{
					outSetting2.active = true;
				}
			}
			return;
		}
		AOMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
		RenderMode = RenderingPath.DeferredShading;
		QualitySettings.shadowResolution = ShadowResolution.Medium;
		if ((bool)postProcessVolume)
		{
			if (postProcessVolume.sharedProfile.TryGetSettings<AmbientOcclusion>(out var outSetting3))
			{
				outSetting3.active = true;
			}
			if (postProcessVolume.sharedProfile.TryGetSettings<MotionBlur>(out var outSetting4))
			{
				outSetting4.active = false;
			}
		}
	}

	public void SetQualityHigh()
	{
		PostProcessVolume postProcessVolume = Object.FindObjectOfType<PostProcessVolume>();
		if (ActiveBuildMode == BuildMode.MOBILE)
		{
			RenderMode = RenderingPath.Forward;
			QualitySettings.shadowResolution = ShadowResolution.Medium;
			QualitySettings.masterTextureLimit = 2;
			if ((bool)postProcessVolume)
			{
				if (postProcessVolume.sharedProfile.TryGetSettings<Bloom>(out var outSetting))
				{
					outSetting.active = true;
				}
				if (postProcessVolume.sharedProfile.TryGetSettings<ColorGrading>(out var outSetting2))
				{
					outSetting2.active = true;
				}
			}
			return;
		}
		AOMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
		RenderMode = RenderingPath.DeferredShading;
		QualitySettings.shadowResolution = ShadowResolution.High;
		if ((bool)postProcessVolume)
		{
			if (postProcessVolume.sharedProfile.TryGetSettings<AmbientOcclusion>(out var outSetting3))
			{
				outSetting3.active = true;
			}
			if (postProcessVolume.sharedProfile.TryGetSettings<MotionBlur>(out var outSetting4))
			{
				outSetting4.active = true;
			}
		}
	}

	public void LoadLaunchQualitySettings()
	{
		PullBuildMode();
		SetQuality();
		if (ActiveBuildMode == BuildMode.PC)
		{
			QualitySettings.vSyncCount = PlayerPrefs.GetInt("VSYNC", 1);
			if ((bool)Camera.main.GetComponent<PostProcessLayer>())
			{
				Camera.main.GetComponent<PostProcessLayer>().antialiasingMode = AOMode;
			}
			for (int i = 0; i < MaterialSwap.Length; i++)
			{
				MaterialSwap[i].SetShader(2);
			}
		}
		else
		{
			for (int j = 0; j < MaterialSwap.Length; j++)
			{
				MaterialSwap[j].SetShader(Mathf.Clamp(PlayerPrefs.GetInt("Quality"), 0, 1));
			}
		}
		Camera.main.renderingPath = RenderMode;
	}

	public void Apply()
	{
		SetQuality();
		if (ActiveBuildMode == BuildMode.PC)
		{
			if ((float)Resolutions.Count > 0f)
			{
				Resolution resolution = Resolutions[(int)s_Resolution.m_Slider.value];
				Screen.SetResolution(resolution.width, resolution.height, m_FullScreen.isOn);
			}
			QualitySettings.vSyncCount = (m_VSync.isOn ? 1 : 0);
			PlayerPrefs.SetInt("VSYNC", QualitySettings.vSyncCount);
			if ((bool)Camera.main.GetComponent<PostProcessLayer>())
			{
				Camera.main.GetComponent<PostProcessLayer>().antialiasingMode = AOMode;
			}
			for (int i = 0; i < MaterialSwap.Length; i++)
			{
				MaterialSwap[i].SetShader(2);
			}
		}
		else
		{
			for (int j = 0; j < MaterialSwap.Length; j++)
			{
				MaterialSwap[j].SetShader(Mathf.Clamp(PlayerPrefs.GetInt("Quality"), 0, 1));
			}
		}
		Camera.main.renderingPath = RenderMode;
		PlayerPrefs.Save();
	}

	public void ChangeMusic()
	{
		PlayerPrefs.SetFloat("Music", s_Music.m_Slider.value);
		MusicManager.MasterMusicVolume = PlayerPrefs.GetFloat("Music", 1f);
		s_Music.m_Value.text = Mathf.Round(s_Music.m_Slider.value * 100f) + "%";
	}

	public void ChangeSound()
	{
		PlayerPrefs.SetFloat("Sound", s_Audio.m_Slider.value);
		MusicManager.MasterSFXVolume = PlayerPrefs.GetFloat("Sound", 1f);
		s_Audio.m_Value.text = Mathf.Round(s_Audio.m_Slider.value * 100f) + "%";
	}
}
