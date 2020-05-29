using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)] public float volume;
    [Range(0f, 1f)] public float pitch;

    [Range(0f, 1f)] public float randomVolume;
    [Range(0f, 1f)] public float randomPitch;

    private AudioSource source;

    public void SetSource(AudioSource source)
    {
        this.source = source;
        source.clip = clip;
    }

    public void Play()
    {
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        source.pitch = volume * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] Sound[] sounds;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            sounds[i].SetSource(go.AddComponent<AudioSource>());
            go.transform.parent = transform;
        }
    }

    public void PlaySound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if(sounds[i].name == name)
            {
                sounds[i].Play();
                return;
            }
        }
    }
}
