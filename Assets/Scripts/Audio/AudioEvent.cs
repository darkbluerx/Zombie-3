using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "AudioEvent", menuName = "Events/Audio Event", order = 2)]
public class AudioEvent : AudioEventBase
{
    public AudioClip[] audioClips;
    [Range(0, 1)] public float Volume = 1;
    [Range(0, 1)] public float RandomizeVolume = 0;

    [Range(0, 3)] public float Pitch = 1;
    [Range(0, 3)] public float RandomizePitch = 0;

#if UNITY_EDITOR
    private AudioSource previewSource;

    private void OnEnable()
    {
        previewSource = EditorUtility.CreateGameObjectWithHideFlags("AudioPreview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        DestroyImmediate(previewSource.gameObject);
    }

    public void PlayFromEditor()
    {
        Play(previewSource);
    }

    public void StopFromEditor()
    {
        previewSource.Stop();
    }
#endif
    public override void Play(AudioSource audioSource)
    {
        if (audioClips == null) return;
        if (audioClips.Length == 0) return;
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.volume = Random.Range(Volume - RandomizeVolume, Volume);
        audioSource.pitch = Random.Range(Pitch - (RandomizePitch / 2), Pitch + RandomizePitch / 2);
        audioSource.PlayOneShot(audioSource.clip, 1f);
    }
}