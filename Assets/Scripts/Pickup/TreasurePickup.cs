using UnityEngine;

[CreateAssetMenu(menuName = "PickupActions/Treasure")]
public class TreasurePickup : PickupAction
{
    public string treasureName;

    public override void Execute(GameObject player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.AddTreasure(treasureName);
            Debug.Log($"Picked up treasure: {treasureName}!");
        }
    }

    public override string GetHintText()
    {
        return $"Press E to pick up {treasureName}";
    }
}