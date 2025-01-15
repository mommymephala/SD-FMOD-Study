using UnityEngine;

[CreateAssetMenu(menuName = "PickupActions/Key")]
public class KeyPickup : PickupAction
{
    public Key key; // Reference to the key ScriptableObject

    public override void Execute(GameObject player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory != null && key != null)
        {
            inventory.AddKey(key);
            Debug.Log($"Picked up key: {key.keyName}");
            AudioManager.instance.PlayRandomSound(AudioManager.instance.pickupSounds);
        }
        else
        {
            Debug.LogWarning("Key or PlayerInventory not found!");
        }
    }

    public override string GetHintText()
    {
        return key != null ? $"Press E to pick up {key.keyName}" : "Press E to pick up key";
    }
}