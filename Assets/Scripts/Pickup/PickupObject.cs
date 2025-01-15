using UnityEngine;

public class PickupObject : MonoBehaviour, IInteractable
{
    public PickupAction pickupAction;

    public void Interact()
    {
        if (pickupAction != null)
        {
            pickupAction.Execute(GameObject.FindWithTag("Player"));
            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("No PickupAction assigned!");
        }
    }

    public string GetHintText()
    {
        return pickupAction != null ? pickupAction.GetHintText() : "Press E to interact";
    }
}