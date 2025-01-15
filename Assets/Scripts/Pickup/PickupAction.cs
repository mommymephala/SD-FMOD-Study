using UnityEngine;

public abstract class PickupAction : ScriptableObject
{
    public abstract void Execute(GameObject player);
    public abstract string GetHintText();
}