using UnityEngine;

[CreateAssetMenu(menuName = "PickupActions/Weapon")]
public class WeaponPickup : PickupAction
{
    public GameObject weaponPrefab;

    public override void Execute(GameObject player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory != null && weaponPrefab != null)
        {
            inventory.AddWeapon(weaponPrefab);
            Debug.Log($"Picked up weapon: {weaponPrefab.name}!");
        }
    }

    public override string GetHintText()
    {
        return "Press E to pick up weapon";
    }
}