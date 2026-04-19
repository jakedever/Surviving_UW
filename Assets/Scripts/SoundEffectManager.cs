using UnityEngine;
using System.Collections.Generic;

public class SoundEffectManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundEffect
    {
        public string soundName;
        public List<AudioClip> clips = new List<AudioClip>();
        public float volume = 1f;
        public float pitchVariation = 0.1f; // Adds variety to pitch
    }

    public List<SoundEffect> soundEffects = new List<SoundEffect>();
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(string soundName)
    {
        SoundEffect effect = soundEffects.Find(s => s.soundName == soundName);
        UnityEngine.Debug.Log("Played sound effect: " + soundName);
        
        if (effect == null)
        {
            UnityEngine.Debug.LogWarning($"Sound effect '{soundName}' not found!");
            return;
        }

        if (effect.clips.Count == 0)
        {
            UnityEngine.Debug.LogWarning($"Sound effect '{soundName}' has no clips assigned!");
            return;
        }



        // Randomly select a clip
        AudioClip randomClip = effect.clips[Random.Range(0, effect.clips.Count)];
        
        // Play the sound
        audioSource.PlayOneShot(randomClip, effect.volume);
        
    }
}
