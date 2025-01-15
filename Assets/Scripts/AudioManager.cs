using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource audioSource;

    public AudioClip[] lockpickSounds;
    public AudioClip chestUnlockSound;
    public AudioClip runeActivation;
    public AudioClip puzzleSolvedSound;
    public AudioClip fireSound;
    public AudioClip[] pickupSounds;
    public AudioClip errorSound;
    public AudioClip doorOpenSound;
    public AudioClip gameOverSound;

    void Awake()
    {
        // Singleton pattern setup
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Keep the AudioManager across scenes
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    // Play random sound from an array (used for lockpicking and pickup)
    public void PlayRandomSound(AudioClip[] clips)
    {
        if (clips.Length == 0) return;

        int randomIndex = Random.Range(0, clips.Length);
        audioSource.PlayOneShot(clips[randomIndex]);
    }

    // Play specific sound (used for chest unlock, error, door open, etc.)
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}