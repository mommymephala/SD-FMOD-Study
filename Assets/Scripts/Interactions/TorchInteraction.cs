using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TorchInteraction : MonoBehaviour, IInteractable
{
    public EventReference fireSoundEvent;
    public EventReference fizzleOutSoundEvent;

    private bool isLit = true;
    private FMOD.Studio.EventInstance fireSoundInstance;

    private void Start() 
    {
        fireSoundInstance = RuntimeManager.CreateInstance(fireSoundEvent);
        fireSoundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        fireSoundInstance.start();
    }

    public void Interact()
    {
        if (isLit)
        {
            ExtinguishTorch();
        }
    }

    private void ExtinguishTorch()
    {
        isLit = false;

        // Stop and release the fire sound instance
        fireSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        fireSoundInstance.release();

        RuntimeManager.PlayOneShot(fizzleOutSoundEvent, transform.position);

        gameObject.SetActive(false);
    }

    public string GetHintText()
    {
        if (isLit)
        {
            return "Press E to extinguish";
        }
        else
        {
            return "";
        }
    }

    private void OnDestroy()
    {
        // Ensure the sound instance is cleaned up
        if (fireSoundInstance.isValid())
        {
            fireSoundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            fireSoundInstance.release();
        }
    }
}