using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
	public static float MasterMusicVolume = 1f;

	public static float MasterSFXVolume = 1f;

	public AudioMixer MusicMixer;

	public AudioMixer SoundMixer;

	internal float SFXModifier = 1f;

	private float ActualModifier;

	public AudioSource SourceA;

	public AudioSource SourceB;

	[Header("Music Tracks")]
	public AudioClip AmbienceTrack;

	public AudioClip ChaseTrack;

	public AudioClip Victory;

	public AudioClip Death;

	[Header("SFX Player")]
	public AudioSource SoundPlayer;

	public AudioSource SoundPlayer2D;

	private bool CurrentAudioSourceIsA = true;

	private float SourceAModifier;

	private float SourceBModifier;

	private float MusicAMultiplier = 1f;

	private float MusicBMultiplier = 1f;

	private bool CrossFade;

	private float TransitionOutSpeed;

	private float TransitionInSpeed;

	private bool FadeOutComplete = true;

	private bool StopOtherTransitions;

	private static MusicManager instance;

	public static MusicManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Object.FindObjectOfType<MusicManager>();
			}
			return instance;
		}
	}

	public void Start()
	{
		MasterMusicVolume = PlayerPrefs.GetFloat("Music", 1f);
		MasterSFXVolume = PlayerPrefs.GetFloat("Sound", 1f);
	}

	public void Update()
	{
		MusicMixer.SetFloat("MusicVolume", LinearToDecibel(MasterMusicVolume * Time.timeScale));
		SoundMixer.SetFloat("SFXVolume", LinearToDecibel(MasterSFXVolume * ActualModifier * Time.timeScale));
		ActualModifier = Mathf.MoveTowards(ActualModifier, SFXModifier, 4f * Time.deltaTime);
		if ((bool)SourceA && (bool)SourceB)
		{
			SourceA.volume = SourceAModifier * MusicAMultiplier;
			SourceB.volume = SourceBModifier * MusicBMultiplier;
			if (CurrentAudioSourceIsA)
			{
				DoCrossFading(ref SourceAModifier, ref SourceBModifier);
			}
			else
			{
				DoCrossFading(ref SourceBModifier, ref SourceAModifier);
			}
		}
	}

	public void ChangeMusic(AudioClip newMusic, float CrossFadeTime, float MaxVolume = 1f, bool StopTrnasitions = false, bool Loop = true)
	{
		if (!StopOtherTransitions)
		{
			StopOtherTransitions = StopTrnasitions;
			CurrentAudioSourceIsA = !CurrentAudioSourceIsA;
			FadeOutComplete = false;
			CrossFade = true;
			TransitionOutSpeed = CrossFadeTime;
			if (CurrentAudioSourceIsA)
			{
				SourceB.clip = newMusic;
				SourceB.loop = Loop;
				MusicBMultiplier = MaxVolume;
			}
			else
			{
				SourceA.clip = newMusic;
				SourceA.loop = Loop;
				MusicAMultiplier = MaxVolume;
			}
		}
	}

	public void ChangeMusicFullZero(AudioClip newMusic, float FadeOutTime, float FadeInTime, float MaxVolume = 1f, bool StopTrnasitions = false, bool Loop = true)
	{
		if (!StopOtherTransitions)
		{
			StopOtherTransitions = StopTrnasitions;
			CurrentAudioSourceIsA = !CurrentAudioSourceIsA;
			CrossFade = false;
			TransitionOutSpeed = FadeOutTime;
			TransitionInSpeed = FadeInTime;
			if (CurrentAudioSourceIsA)
			{
				SourceB.clip = newMusic;
				SourceB.loop = Loop;
				MusicBMultiplier = MaxVolume;
			}
			else
			{
				SourceA.clip = newMusic;
				SourceA.loop = Loop;
				MusicAMultiplier = MaxVolume;
			}
		}
	}

	private void DoCrossFading(ref float FromVol, ref float ToVol)
	{
		AudioSource audioSource;
		AudioSource audioSource2;
		if (CurrentAudioSourceIsA)
		{
			audioSource = SourceA;
			audioSource2 = SourceB;
		}
		else
		{
			audioSource = SourceB;
			audioSource2 = SourceA;
		}
		if (CrossFade)
		{
			if (FromVol > 0f)
			{
				FromVol -= TransitionOutSpeed * Time.deltaTime;
			}
			else if (audioSource.isPlaying)
			{
				audioSource.Stop();
			}
			if (ToVol < 1f)
			{
				ToVol += TransitionOutSpeed * Time.deltaTime;
				if (!audioSource2.isPlaying)
				{
					audioSource2.Play();
				}
			}
		}
		else if (!FadeOutComplete)
		{
			if (FromVol > 0f)
			{
				FromVol -= TransitionOutSpeed * Time.deltaTime;
			}
			else
			{
				FadeOutComplete = true;
			}
		}
		else
		{
			if (audioSource.isPlaying)
			{
				audioSource.Stop();
			}
			if (!audioSource2.isPlaying)
			{
				audioSource2.Play();
			}
			if (ToVol < 1f)
			{
				ToVol += TransitionInSpeed * Time.deltaTime;
			}
		}
		if (ToVol >= 1f && StopOtherTransitions)
		{
			StopOtherTransitions = false;
		}
	}

	private void UpdateVolumes(float from, float to)
	{
		if (CurrentAudioSourceIsA)
		{
			SourceAModifier = from;
			SourceBModifier = to;
		}
		else
		{
			SourceBModifier = from;
			SourceAModifier = to;
		}
	}

	public void Play2dSound(AudioClip clip)
	{
		SoundPlayer2D.PlayOneShot(clip);
	}

	public void PlaySoundAtPosition(AudioClip clip, Vector3 position)
	{
		SoundPlayer.transform.position = position;
		SoundPlayer.PlayOneShot(clip);
	}

	private float LinearToDecibel(float linear)
	{
		if (linear != 0f)
		{
			return 20f * Mathf.Log10(linear);
		}
		return -144f;
	}
}
