using UnityEngine;

/// <summary>
/// This class is used to store an item in the world.
/// This is added to a gameobject which has a reference to an Item(ScriptableObject).
/// The gameobject also has a collider which allows it to be picked by a character.
/// </summary>
public class CollectableItem : MonoBehaviour
{
    public Item Item;
}
