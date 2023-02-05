using System.Collections.Generic;
using UnityEngine;

public class BaseAudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private Dictionary<string, Sound> soundsByName;

    void Start()
    {
        soundsByName = new Dictionary<string, Sound>(sounds.Length);
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            soundsByName.Add(s.name, s);
        }        
    }

    public void Play(string name)
    {
        if(soundsByName.TryGetValue(name, out Sound targetSound))
        {
            targetSound.source.Play();
        }
        else
        {
            Debug.LogWarning($"Sound {name} not found on {this.gameObject.name}");
        }
    }
}

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0, 1)]
    public float volume;
    public float pitch;
    [Range(0, 1)]
    public float dimensionality;

    public void SetSource(AudioSource src)
    {
        this.source = src;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.spatialBlend = dimensionality;
        source.playOnAwake = false;
    }

    [HideInInspector]
    public AudioSource source;
}
