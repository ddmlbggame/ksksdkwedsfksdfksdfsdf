using System;
using System.Collections.Generic;
using UnityEngine;

public class FSoundManager
{
	public static string resourcePrefix = "Audio/";

	private static GameObject _gameObject;

	private static AudioSource _soundSource;

	private static AudioSource _musicSource;

	private static bool _highPassFilterOn = false;

	private static bool _chorusFilterOn = false;

	private static bool _musicPitchIsLowered = false;

	private static string _currentMusicPath = string.Empty;

	private static Dictionary<string, AudioClip> _soundClips = new Dictionary<string, AudioClip>();

	private static AudioClip _currentMusicClip = null;

	private static float _volume = 1f;

	public static float musicPitch
	{
		get
		{
			return FSoundManager._musicSource.pitch;
		}
		set
		{
			FSoundManager._musicSource.pitch = value;
		}
	}

	public static bool highPassFilterOn
	{
		get
		{
			return FSoundManager._highPassFilterOn;
		}
	}

	public static bool musicPitchIsLowered
	{
		get
		{
			return FSoundManager._musicPitchIsLowered;
		}
	}

	public static bool chorusFilterOn
	{
		get
		{
			return FSoundManager._chorusFilterOn;
		}
		set
		{
			FSoundManager._chorusFilterOn = value;
			GameObject gameObject = GameObject.Find("Audio Listener");
			AudioChorusFilter component = gameObject.GetComponent<AudioChorusFilter>();
			component.enabled = FSoundManager._chorusFilterOn;
		}
	}

	public static float musicSourceTime
	{
		get
		{
			return FSoundManager._musicSource.time;
		}
		set
		{
			FSoundManager._musicSource.time = value;
		}
	}

	public static float musicSourceLength
	{
		get
		{
			return FSoundManager._musicSource.clip.length;
		}
	}

	public static float volume
	{
		get
		{
			return AudioListener.volume;
		}
		set
		{
			FSoundManager._volume = value;
			if (AudioListener.pause)
			{
				AudioListener.volume = 0f;
			}
			else
			{
				AudioListener.volume = FSoundManager._volume;
			}
		}
	}

	public static bool isMuted
	{
		get
		{
			return AudioListener.pause;
		}
		set
		{
			AudioListener.pause = value;
			PlayerPrefs.SetInt("FSoundManager_IsAudioMuted", (!value) ? 0 : 1);
			if (AudioListener.pause)
			{
				AudioListener.volume = 0f;
			}
			else
			{
				AudioListener.volume = FSoundManager._volume;
			}
		}
	}

	public static void Init()
	{
		FSoundManager._gameObject = new GameObject("FSoundManager");
		FSoundManager._musicSource = FSoundManager._gameObject.AddComponent<AudioSource>();
		FSoundManager._soundSource = FSoundManager._gameObject.AddComponent<AudioSource>();
		if (PlayerPrefs.HasKey("SoundMute"))
		{
			FSoundManager._soundSource.mute = PlayerPrefs.GetInt("SoundMute") == 1;
		}
		if (PlayerPrefs.HasKey("MusicMute"))
		{
			FSoundManager._musicSource.mute = PlayerPrefs.GetInt("MusicMute") == 1;
		}

		if (PlayerPrefs.HasKey("FSoundManager_IsAudioMuted"))
		{
			FSoundManager.isMuted = (PlayerPrefs.GetInt("FSoundManager_IsAudioMuted") == 1);
		}
	}

	//public static void TurnHighPassFilterOn(float dur, float frequency = 5000f)
	//{
	//	GameObject gameObject = GameObject.Find("Audio Listener");
	//	AudioHighPassFilter component = gameObject.GetComponent<AudioHighPassFilter>();
	//	FSoundManager._highPassFilterOn = true;
	//	Go.killAllTweensWithTarget(component);
	//	Go.to(component, dur, new GoTweenConfig().floatProp("cutoffFrequency", frequency, false));
	//}

	//public static void TurnHighPassFilterOff(float dur)
	//{
	//	GameObject gameObject = GameObject.Find("Audio Listener");
	//	AudioHighPassFilter component = gameObject.GetComponent<AudioHighPassFilter>();
	//	FSoundManager._highPassFilterOn = false;
	//	Go.killAllTweensWithTarget(component);
	//	Go.to(component, dur, new GoTweenConfig().floatProp("cutoffFrequency", 10f, false));
	//}

	//public static void LowerMusicPitch(float dur, float pitch)
	//{
	//	if (FSoundManager._musicPitchIsLowered)
	//	{
	//		return;
	//	}
	//	FSoundManager._musicPitchIsLowered = true;
	//	Go.killAllTweensWithTarget(FSoundManager._musicSource);
	//	Go.to(FSoundManager._musicSource, dur, new GoTweenConfig().floatProp("pitch", pitch, false));
	//}

	//public static void ReturnMusicPitchToNormal(float dur)
	//{
	//	if (!FSoundManager._musicPitchIsLowered)
	//	{
	//		return;
	//	}
	//	FSoundManager._musicPitchIsLowered = false;
	//	Go.killAllTweensWithTarget(FSoundManager._musicSource);
	//	Go.to(FSoundManager._musicSource, dur, new GoTweenConfig().floatProp("pitch", 1f, false));
	//}

	public static void SetResourcePrefix(string prefix)
	{
		FSoundManager.resourcePrefix = prefix;
	}

	public static void PreloadSound(string resourceName)
	{
		string text = FSoundManager.resourcePrefix + resourceName;
		if (FSoundManager._soundClips.ContainsKey(text))
		{
			return;
		}
		AudioClip audioClip = Resources.Load(text) as AudioClip;
		if (audioClip == null)
		{
			Debug.Log("Couldn't find sound at: " + text);
		}
		else
		{
			FSoundManager._soundClips[text] = audioClip;
		}
	}

	public static void PlaySound(string resourceName, float volume)
	{
		if (FSoundManager._soundSource == null)
		{
			FSoundManager.Init();
		}
		string text = FSoundManager.resourcePrefix + resourceName;
		AudioClip audioClip;
		if (FSoundManager._soundClips.ContainsKey(text))
		{
			audioClip = FSoundManager._soundClips[text];
		}
		else
		{
			audioClip = (Resources.Load(text) as AudioClip);
			if (audioClip == null)
			{
				Debug.Log("Couldn't find sound at: " + text);
				return;
			}
			FSoundManager._soundClips[text] = audioClip;
		}
		FSoundManager._soundSource.PlayOneShot(audioClip, volume);
	}

	public static void PlaySound(string resourceName)
	{
		FSoundManager.PlaySound(resourceName, 1f);
	}

	public static void PlayMusic(string resourceName, float volume)
	{
		FSoundManager.PlayMusic(resourceName, volume, true);
	}

	public static void PlayMusic(string resourceName, float volume, bool shouldRestartIfSameSongIsAlreadyPlaying)
	{
		if (FSoundManager._musicSource == null)
		{
			FSoundManager.Init();
		}
		string text = FSoundManager.resourcePrefix + resourceName;
		if (FSoundManager._currentMusicClip != null)
		{
			if (FSoundManager._currentMusicPath == text)
			{
				if (shouldRestartIfSameSongIsAlreadyPlaying)
				{
					FSoundManager._musicSource.Stop();
					FSoundManager._musicSource.volume = volume;
					FSoundManager._musicSource.loop = true;
					FSoundManager._musicSource.Play();
				}
				return;
			}
			FSoundManager._musicSource.Stop();
			Resources.UnloadAsset(FSoundManager._currentMusicClip);
			FSoundManager._currentMusicClip = null;
		}
		FSoundManager._currentMusicClip = (Resources.Load(text) as AudioClip);
		if (FSoundManager._currentMusicClip == null)
		{
			Debug.Log("Error! Couldn't find music clip " + text);
		}
		else
		{
			FSoundManager._currentMusicPath = text;
			FSoundManager._musicSource.clip = FSoundManager._currentMusicClip;
			FSoundManager._musicSource.volume = volume;
			FSoundManager._musicSource.loop = true;
			FSoundManager._musicSource.Play();
		}
	}

	public static void PlayMusic(string resourceName)
	{
		FSoundManager.PlayMusic(resourceName, 1f);
	}

	public static void StopMusic()
	{
		Debug.Log("stop music");
		if (FSoundManager._musicSource != null)
		{
			FSoundManager._musicSource.Stop();
		}
	}

	public static void UnloadSound(string resourceName)
	{
		string key = FSoundManager.resourcePrefix + resourceName;
		if (FSoundManager._soundClips.ContainsKey(key))
		{
			AudioClip assetToUnload = FSoundManager._soundClips[key];
			Resources.UnloadAsset(assetToUnload);
			FSoundManager._soundClips.Remove(key);
		}
	}

	public static void UnloadMusic()
	{
		if (FSoundManager._currentMusicClip != null)
		{
			Resources.UnloadAsset(FSoundManager._currentMusicClip);
			FSoundManager._currentMusicClip = null;
			FSoundManager._currentMusicPath = string.Empty;
		}
	}

	public static void UnloadAllSounds()
	{
		foreach (AudioClip current in FSoundManager._soundClips.Values)
		{
			Resources.UnloadAsset(current);
		}
		FSoundManager._soundClips.Clear();
	}

	public static void UnloadAllSoundsAndMusic()
	{
		FSoundManager.UnloadAllSounds();
		FSoundManager.UnloadMusic();
	}

	public static bool IsSoundMute
	{
		get
		{
			if (PlayerPrefs.HasKey("SoundMute"))
			{
				return (PlayerPrefs.GetInt("SoundMute") == 1);
			}
			return false;
		}
		set
		{
			int mute = value ? 1 : 0;
			PlayerPrefs.SetInt("SoundMute", mute);
			FSoundManager._soundSource.mute = value;
		}
	}

	public static bool IsMusicMute
	{
		get
		{
			if (PlayerPrefs.HasKey("MusicMute"))
			{
				return (PlayerPrefs.GetInt("MusicMute") == 1);
			}
			return false;
		}
		set
		{
			int mute = value ? 1 : 0;
			PlayerPrefs.SetInt("MusicMute", mute);
			FSoundManager._musicSource.mute = value;
		}
	}
}
