using UnityEngine;

[CreateAssetMenu(menuName = "PickupActions/Lockpick")]
public class LockpickPickup : PickupAction
{
    public int quantity = 1;

    public override void Execute(GameObject player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.AddLockpicks(quantity);
            Debug.Log($"Picked up {quantity} lockpick(s)!");
            AudioManager.instance.PlayRandomSound(AudioManager.instance.pickupSounds);
        }
    }

    public override string GetHintText()
    {
        return "Press E to pick up lockpicks";
    }
}