using UnityEngine;

[CreateAssetMenu(fileName = "NewKey", menuName = "Inventory/Key")]
public class Key : ScriptableObject
{
    public string keyName;
    public Sprite keyIcon;
}