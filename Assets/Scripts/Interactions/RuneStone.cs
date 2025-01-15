using UnityEngine;

public class RuneStone : MonoBehaviour
{
    public bool isActivated = false;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Fireball") && !isActivated)
        {
            ActivateRune();
        }
    }

    public void ActivateRune()
    {
        isActivated = true;
        AudioManager.instance?.PlaySound(AudioManager.instance.runeActivationEvent);

        RunicPuzzleManager.Instance.RuneActivated();
    }
}