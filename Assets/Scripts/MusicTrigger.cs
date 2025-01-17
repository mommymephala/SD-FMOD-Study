using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    private AudioManager audioManager;
    public bool isOnFirstFloor = true;
    private void Start()
    {

        audioManager = AudioManager.instance;

        if (audioManager == null)
        {
            Debug.LogError("AudioManager null");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && audioManager != null)
        {

            if (!isOnFirstFloor)
            {
                audioManager.firstFloorSnapshot.start();

                audioManager.secondFloorSnapshot.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

                audioManager.firstFloorSnapshot.setParameterByName("Intensity", 1.0f);
                audioManager.secondFloorSnapshot.setParameterByName("Intensity", 0.0f);


                audioManager.firstFloorAmbience.setVolume(1.0f);
                audioManager.firstFloorMusic.setVolume(1.0f);

                audioManager.secondFloorAmbience.setVolume(0.0f);
                audioManager.secondFloorMusic.setVolume(0.0f);
                isOnFirstFloor = true;
            }
            else if (isOnFirstFloor)
            {

                audioManager.secondFloorSnapshot.start();

                audioManager.firstFloorSnapshot.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                audioManager.firstFloorSnapshot.setParameterByName("Intensity", 0.0f);
                audioManager.secondFloorSnapshot.setParameterByName("Intensity", 1.0f);


                audioManager.secondFloorAmbience.setVolume(1.0f);
                audioManager.secondFloorMusic.setVolume(1.0f);

                audioManager.firstFloorAmbience.setVolume(0.0f);
                audioManager.firstFloorMusic.setVolume(0.0f);

                isOnFirstFloor = false;
            }
        }
    }
}