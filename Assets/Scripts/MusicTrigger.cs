using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public bool playTrack1; // Set true for returning to track 1, false for switching to track 2

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playTrack1)
            {
                AudioManager.instance.PlayMusicAndAmbiance(AudioManager.instance.musicTrack1, AudioManager.instance.ambianceTrack1);
            }
            else
            {
                AudioManager.instance.PlayMusicAndAmbiance(AudioManager.instance.musicTrack2, AudioManager.instance.ambianceTrack2);
            }
        }
    }
}