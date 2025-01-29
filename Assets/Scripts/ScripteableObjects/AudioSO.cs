
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioSO", menuName = "ScriptableObjects/AudioSO", order = 1)]
public class AudioSO : ScriptableObject
{
	public List<AudioObject> audioObjects;
}

[Serializable]
public class AudioObject
{
	public AudioType type;
	public bool multiple;
	public List<AudioClip> clips;
	public AudioClip Clip => clips[0];

	public AudioObject(params AudioClip[] clips)
	{
		this.clips = new List<AudioClip>(clips);
		multiple = clips.Length > 1;
	}
}