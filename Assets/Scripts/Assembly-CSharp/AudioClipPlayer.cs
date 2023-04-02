using UnityEngine;

public class AudioClipPlayer : MonoBehaviour
{
	public AudioClip[] Clips;

	public float Volume = 1f;

	private AudioSource Source;

	public bool SetParent = true;

	public bool is2d;

	public bool StopIfIsPlaying;

	private Animator ConnetedAnimator;

	private bool CanPlayClip = true;

	private bool initialized;

	public bool IsPlaying
	{
		get
		{
			if ((bool)Source)
			{
				return Source.isPlaying;
			}
			return false;
		}
	}

	public void Start()
	{
		Init();
	}

	public void Init()
	{
		if (!initialized)
		{
			ConnetedAnimator = GetComponent<Animator>();
			Source = Object.Instantiate(MusicManager.Instance.SoundPlayer);
			Source.transform.position = base.transform.position;
			if (SetParent)
			{
				Source.transform.SetParent(base.transform);
			}
			Source.volume = Volume;
			if (is2d)
			{
				Source.spatialBlend = 0f;
			}
			initialized = true;
		}
	}

	public void LateUpdate()
	{
		if (!ConnetedAnimator)
		{
			return;
		}
		float @float = ConnetedAnimator.GetFloat("PlayClip");
		if (@float > 1f)
		{
			if (CanPlayClip)
			{
				PlayClip();
				CanPlayClip = false;
			}
		}
		else if (@float < -1f)
		{
			CanPlayClip = true;
		}
	}

	public void PlayClip()
	{
		if (!StopIfIsPlaying || !Source.isPlaying)
		{
			Source.pitch = 1f;
			Source.PlayOneShot(Clips[Random.Range(0, Clips.Length)]);
		}
	}

	public void PlayClip(int index)
	{
		Source.pitch = 1f;
		Source.PlayOneShot(Clips[index]);
	}

	public void PlayClip(int index, float PitchShiftDelta)
	{
		Source.pitch = 1f + Random.Range(0f - PitchShiftDelta, PitchShiftDelta);
		Source.PlayOneShot(Clips[index]);
	}
}
