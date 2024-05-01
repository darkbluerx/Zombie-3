using UnityEngine;

public abstract class AudioEventBase : ScriptableObject
{
    public abstract void Play(AudioSource audioSource);
}
