using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    

    [Header("FMOD Event References")]
    public FMODUnity.EventReference firstFloorAmbienceEventReference;
    public FMODUnity.EventReference secondFloorAmbienceEventReference;

    public FMODUnity.EventReference firstFloorMusicEventReference;
    public FMODUnity.EventReference secondFloorMusicEventReference;

    // Snapshot referanslarý
    public FMODUnity.EventReference firstFloorSnapshotReference;
    public FMODUnity.EventReference secondFloorSnapshotReference;

    public FMOD.Studio.EventInstance firstFloorAmbience;
    public FMOD.Studio.EventInstance secondFloorAmbience;

    public FMOD.Studio.EventInstance firstFloorMusic;
    public FMOD.Studio.EventInstance secondFloorMusic;


    public FMOD.Studio.EventInstance firstFloorSnapshot;
    public FMOD.Studio.EventInstance secondFloorSnapshot;



    float intensity; // Intensity deðeri (snapshot blending için)
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

        firstFloorSnapshot = FMODUnity.RuntimeManager.CreateInstance("snapshot:/FirstFloor");
        secondFloorSnapshot = FMODUnity.RuntimeManager.CreateInstance("snapshot:/SecondFloor");

        // Ýlk snapshot'ý baþlat
        firstFloorSnapshot.start();
    }

    private void Start()
    {
        //PlayMusicAndAmbiance(musicTrack1, ambianceTrack1);
        firstFloorAmbience = FMODUnity.RuntimeManager.CreateInstance(firstFloorAmbienceEventReference);
        secondFloorAmbience = FMODUnity.RuntimeManager.CreateInstance(secondFloorAmbienceEventReference);

        firstFloorMusic = FMODUnity.RuntimeManager.CreateInstance(firstFloorMusicEventReference);
        secondFloorMusic = FMODUnity.RuntimeManager.CreateInstance(secondFloorMusicEventReference);

        // Snapshot instance'larýný oluþtur
        firstFloorSnapshot = FMODUnity.RuntimeManager.CreateInstance(firstFloorSnapshotReference);
        secondFloorSnapshot = FMODUnity.RuntimeManager.CreateInstance(secondFloorSnapshotReference);

        // Tüm event'leri baþlat
        firstFloorAmbience.start();
        secondFloorAmbience.start();
        firstFloorMusic.start();
        secondFloorMusic.start();


        secondFloorAmbience.setVolume(0.0f);
        secondFloorMusic.setVolume(0.0f);
        // Baþlangýçta alt kat (First Floor) snapshot'ýný etkinleþtir
        firstFloorSnapshot.start();
    }
   
    /*void Update()
    {
        // Kullanýcý giriþine baðlý olarak snapshot kontrolü
        if (Input.GetKey(KeyCode.F)) // "F" tuþuna basýldýðýnda alt kat (First Floor)
        {
            
            firstFloorSnapshot.setParameterByName("Intensity", 1.0f);
            secondFloorSnapshot.setParameterByName("Intensity", 0.0f);

            
            firstFloorAmbience.setVolume(1.0f);
            firstFloorMusic.setVolume(1.0f);

            secondFloorAmbience.setVolume(0.0f);
            secondFloorMusic.setVolume(0.0f);
        }
        else if (Input.GetKey(KeyCode.S)) // "S" tuþuna basýldýðýnda üst kat (Second Floor)
        {
            
            firstFloorSnapshot.setParameterByName("Intensity", 0.0f);
            secondFloorSnapshot.setParameterByName("Intensity", 1.0f);

          
            secondFloorAmbience.setVolume(1.0f);
            secondFloorMusic.setVolume(1.0f);

            firstFloorAmbience.setVolume(0.0f);
            firstFloorMusic.setVolume(0.0f);
        }
    }*/
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
    
    /* public void PlayMusicAndAmbiance(EventReference musicEvent, EventReference ambianceEvent)
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
     }*/

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

    //------------------------------------------------------------------------------------------

     /*public void ActivateFirstFloor()
      {
          if (!isOnFirstFloor)
          {
              Debug.Log("Activating First Floor Snapshot...");
              FMOD.RESULT result = firstFloorSnapshotInstance.start();
              Debug.Log($"First Floor Snapshot Start Result: {result}");

              secondFloorSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
              isOnFirstFloor = true;
          }
      }

      public void ActivateSecondFloor()
      {
          if (isOnFirstFloor)
          {
              Debug.Log("Activating Second Floor Snapshot...");
              FMOD.RESULT result = secondFloorSnapshotInstance.start();
              Debug.Log($"Second Floor Snapshot Start Result: {result}");

              firstFloorSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
              isOnFirstFloor = false;
          }
      }

      private void OnDestroy()
      {
          // Kaynaklarý serbest býrak
          firstFloorSnapshotInstance.release();
          secondFloorSnapshotInstance.release();
      }*/

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