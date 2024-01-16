using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


/// <summary>
/// This class is responsible for storing the items that the character has collected.
/// Each character has their own inventory.
/// Charater component is required.
/// </summary>
public class Inventory : MonoBehaviour
{
    private Character character; // Reference to the character component on this game object.

    public Dictionary<Item, int> inventory = new(); // Dictionary of items and their quantities.

    private void Awake()
    {
        character = GetComponent<Character>();
        if (character == null)
        {
            Debug.LogError("Character component not found on " + gameObject.name);
        }
    }

    [ContextMenu("Save Inventory")]
    internal void SaveInventory()
    {
        Save(inventory, character.characterName + "inventory.json");
    }

    [ContextMenu("Load Inventory")]
    internal void LoadInventory()
    {
        inventory = Load(character.characterName + "inventory.json");
    }


    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item]++;
        }
        else
        {
            inventory.Add(item, 1);
        }
    }


    /// <summary>
    /// Equips or uses an item from the inventory.
    /// </summary>
    /// <param name="item"></param>
    public void EquipUseItem(Item item)
    {
        if (inventory.ContainsKey(item))
        {
            if (item.Type == Item.ItemCategory.Equipment)
            {
                character.EquipItem(item);
            }
            else if (item.Type == Item.ItemCategory.Consumable)
            {
                character.UseItem(item);
                RemoveItem(item);
            }
        }
    }


    /// <summary>
    /// Drops an item from the inventory.
    /// </summary>
    /// <param name="item"></param>
    public void DropItem(Item item)
    {
        if (inventory.ContainsKey(item))
        {
            character.DropItem(item);
            RemoveItem(item);
        }
    }
    

    /// <summary>
    /// Removes an item from the inventory.
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(Item item)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item]--;
            if (inventory[item] <= 0)
            {
                inventory.Remove(item);
            }
        }
    }

    public static void Save(Dictionary<Item, int> dictionary, string filename)
    {
        SerializableInventory serializableDictionary = new SerializableInventory();
        foreach (KeyValuePair<Item, int> pair in dictionary)
        {
            serializableDictionary.keys.Add(pair.Key);
            serializableDictionary.values.Add(pair.Value);
        }

        string json = JsonUtility.ToJson(serializableDictionary);
        System.IO.File.WriteAllText(Application.dataPath + "/" + filename, json);
    }

    public static Dictionary<Item, int> Load(string filename)
    {
        Dictionary<Item, int> dictionary = new Dictionary<Item, int>();
        string filePath = Application.dataPath + "/" + filename;

        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            SerializableInventory serializableDictionary = JsonUtility.FromJson<SerializableInventory>(json);

            for (int i = 0; i < serializableDictionary.keys.Count; i++)
            {
                dictionary.Add(serializableDictionary.keys[i], serializableDictionary.values[i]);
            }
        }
        else
        {
            Debug.LogError("Cannot find file.");
        }

        return dictionary;
    }
}


[System.Serializable]
/// <summary>
/// A wrapper class for serializing the inventory.
/// </summary>
public class SerializableInventory
{
    public List<Item> keys = new List<Item>();
    public List<int> values = new List<int>();
}