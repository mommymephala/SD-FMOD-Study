using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("FMOD Event References")]
    public EventReference lockpickSoundEvents;
    public EventReference pickupSoundEvents;
    public EventReference chestUnlockEvent;
    public EventReference runeActivationEvent;
    public EventReference puzzleSolvedEvent;
    public EventReference fireEvent;
    public EventReference errorEvent;
    public EventReference doorOpenEvent;
    public EventReference gameOverEvent;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(EventReference eventReference)
    {
        if (eventReference.IsNull) return;

        RuntimeManager.PlayOneShot(eventReference);
    }

    public void PlaySoundAtPosition(EventReference eventReference, Vector3 position)
    {
        if (eventReference.IsNull) return;

        RuntimeManager.PlayOneShot(eventReference, position);
    }

    // Example of playing a sound with parameters (optional enhancement)
    public void PlaySoundWithParameter(EventReference eventReference, string parameterName, float parameterValue)
    {
        if (eventReference.IsNull) return;

        EventInstance instance = RuntimeManager.CreateInstance(eventReference);
        instance.setParameterByName(parameterName, parameterValue);
        instance.start();
        instance.release(); // Release the instance after playback
    }
}