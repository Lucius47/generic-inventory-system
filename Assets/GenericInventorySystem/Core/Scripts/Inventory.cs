using System.Collections.Generic;
using UnityEngine;

namespace GIS
{
    /// <summary>
    /// This component can be added to any GameObject to have its own inventory.
    /// It is responsible for holding the inventory state during and between sessions.
    /// </summary>
    [System.Serializable]
    public class Inventory : MonoBehaviour
    {
        private string characterName;

        internal Dictionary<Item, int> inventory = new(); // Dictionary of items and their quantities.

        //[ContextMenu("Save Inventory")]
        internal void SaveInventory()
        {
            Save(inventory, characterName + "inventory.json");
        }

        //[ContextMenu("Load Inventory")]
        internal void LoadInventory(string _name)
        {
            characterName = _name;
            inventory = Load(characterName + "inventory.json");
        }


        /// <summary>
        /// Adds an item to the inventory.
        /// </summary>
        /// <param name="item"></param>
        internal void AddItem(Item item)
        {
            if (ContainsItem(item))
            {
                inventory[item]++;
            }
            else
            {
                inventory.Add(item, 1);
            }

            Save(inventory, characterName + "inventory.json");
        }


        internal bool ContainsItem(Item item)
        {
            return inventory.ContainsKey(item);
        }


        /// <summary>
        /// Removes an item from the inventory.
        /// </summary>
        /// <param name="item"></param>
        internal void RemoveItem(Item item)
        {
            if (ContainsItem(item))
            {
                inventory[item]--;
                if (inventory[item] <= 0)
                {
                    inventory.Remove(item);
                }

                Save(inventory, characterName + "inventory.json");
            }
        }

        /*public*/private static void Save(Dictionary<Item, int> dictionary, string filename)
        {
            SerializableInventory serializableDictionary = new SerializableInventory(dictionary.Count);

            int i = 0;

            foreach (KeyValuePair<Item, int> pair in dictionary)
            {
                serializableDictionary.keys[i] = pair.Key;
                serializableDictionary.values[i] = pair.Value;
                i++;
            }

            string json = JsonUtility.ToJson(serializableDictionary);
            System.IO.File.WriteAllText(Application.dataPath + "/" + filename, json);
        }

        /*public*/private static Dictionary<Item, int> Load(string filename)
        {
            Dictionary<Item, int> dictionary = new Dictionary<Item, int>();
            string filePath = Application.dataPath + "/" + filename;

            if (System.IO.File.Exists(filePath))
            {
                string json = System.IO.File.ReadAllText(filePath);
                SerializableInventory serializableDictionary = JsonUtility.FromJson<SerializableInventory>(json);

                for (int i = 0; i < serializableDictionary.keys.Length; i++)
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


    /// <summary>
    /// Since JsonUtility cannot serialize a Dictionary, This is a wrapper class to serialize it as two arrays.
    /// </summary>
    [System.Serializable]
    public class SerializableInventory
    {
        public Item[] keys;
        public int[] values;

        public SerializableInventory(int numOfPairs)
        {
            keys = new Item[numOfPairs];
            values = new int[numOfPairs];
        }
    }
}
