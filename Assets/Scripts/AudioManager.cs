using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("FMOD Event References")]

    public EventReference musicTrack1;
    public EventReference musicTrack2;
    public EventReference ambianceTrack1;
    public EventReference ambianceTrack2;
    private EventInstance musicInstance;
    private EventInstance ambianceInstance;
    
    public EventReference footstepEvent;
    public EventReference lockpickSoundEvents;
    public EventReference lockpickUnlockEvent;
    public EventReference pickupSoundEvents;
    public EventReference chestUnlockEvent;
    public EventReference runeActivationEvent;
    public EventReference puzzleSolvedEvent;
    public EventReference fireEvent;
    public EventReference errorEvent;
    public EventReference doorOpenEvent;
    public EventReference doorUnlockEvent;
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

    private void Start()
    {
        PlayMusicAndAmbiance(musicTrack1, ambianceTrack1);
    }

    public void PlayFootstep(string surfaceType, Vector3 position)
    {
        if (footstepEvent.IsNull) return;

        EventInstance footstepInstance = RuntimeManager.CreateInstance(footstepEvent);
        footstepInstance.setParameterByName("Surfaces", SurfaceTypeToParameter(surfaceType));
        footstepInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        footstepInstance.start();
        footstepInstance.release(); // Release after playback
    }

    private float SurfaceTypeToParameter(string surfaceType)
    {
        switch (surfaceType)
        {
            case "Stone": return 0f;
            case "Carpet": return 1f;
            case "Sand": return 2f;
            default: return -1f; // Undefined surface
        }
    }

    public void PlayMusicAndAmbiance(EventReference musicEvent, EventReference ambianceEvent)
    {
        StopCurrentTracks();

        if (!musicEvent.IsNull)
        {
            musicInstance = RuntimeManager.CreateInstance(musicEvent);
            musicInstance.start();
        }

        if (!ambianceEvent.IsNull)
        {
            ambianceInstance = RuntimeManager.CreateInstance(ambianceEvent);
            ambianceInstance.start();
        }
    }

    public void StopCurrentTracks()
    {
        if (musicInstance.isValid())
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicInstance.release();
        }

        if (ambianceInstance.isValid())
        {
            ambianceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            ambianceInstance.release();
        }
    }

    public void PauseTracks()
    {
        if (musicInstance.isValid())
            musicInstance.setPaused(true);

        if (ambianceInstance.isValid())
            ambianceInstance.setPaused(true);
    }

    public void ResumeTracks()
    {
        if (musicInstance.isValid())
            musicInstance.setPaused(false);

        if (ambianceInstance.isValid())
            ambianceInstance.setPaused(false);
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