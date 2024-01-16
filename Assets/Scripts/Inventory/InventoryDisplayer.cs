using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This class is responsible for displaying the items in an inventory.
/// </summary>
public class InventoryDisplayer : MonoBehaviour
{
    private Inventory displayedInventory; // The inventory that is currently being displayed.

    [SerializeField] private GameObject itemDisplayerPrefab; // The prefab that is used to display the items in the inventory.
    [SerializeField] private Transform itemsTransform; // The transform that holds the items in the inventory.

    [SerializeField] private TMP_Dropdown sortDropdown; // The dropdown that is used to sort the items in the inventory.
    [SerializeField] private TMP_Dropdown filterDropdown; // The dropdown that is used to filter the items in the inventory.


    private ItemDisplayer[] allInventoryItems; // All the items in the inventory.
    private ItemDisplayer[] sortedInventoryItems; // The items in the inventory that are sorted.
    private List<ItemDisplayer> filteredInventoryItems; // The items in the inventory that are filtered.

    [SerializeField] private SelectedItemInfoDisplayer selectedItemInfoDisplayer; // The script that is responsible for displaying the selected item info.

    private void Awake()
    {
        sortDropdown.onValueChanged.AddListener(SortInventory); //delegate { SortInventory(sortDropdown.value); }
        filterDropdown.onValueChanged.AddListener(FilterInventory);
    }

    /// <summary>
    /// Filters the inventory items based on the selected value.
    /// </summary>
    /// <param name="value"></param>
    private void FilterInventory(int value)
    {
        // 0 = None
        // 1 = Equipment
        // 2 = Consumable
        // 3 = Quest Item

        filteredInventoryItems = new List<ItemDisplayer>();

        switch (value)
        {
            case 0:
                foreach (var item in allInventoryItems)
                {
                    filteredInventoryItems.Add(item);
                }
                break;

            case 1:
                foreach (var item in allInventoryItems)
                {
                    if (item.DisplayedItem.Type == Item.ItemCategory.Equipment)
                    {
                        filteredInventoryItems.Add(item);
                    }
                }

                break;

            case 2:
                foreach (var item in allInventoryItems)
                {
                    if (item.DisplayedItem.Type == Item.ItemCategory.Consumable)
                    {
                        filteredInventoryItems.Add(item);
                    }
                }

                break;

            case 3:
                foreach (var item in allInventoryItems)
                {
                    if (item.DisplayedItem.Type == Item.ItemCategory.Quest)
                    {
                        filteredInventoryItems.Add(item);
                    }
                }

                break;
        }

        foreach (var item in allInventoryItems)
        {
            item.gameObject.SetActive(false);
        }

        foreach (var item in filteredInventoryItems)
        {
            item.gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// Sorts the inventory items based on the selected value.
    /// </summary>
    /// <param name="value"></param>
    private void SortInventory(int value)
    {
        // 0 = None
        // 1 = Name
        // 2 = Type
        // 3 = Value
        // 4 = Weight
        // 5 = Rarity

        sortedInventoryItems = allInventoryItems;

        switch (value)
        {
            case 0:
                break;

            case 1:
                // sort allInventoryItems by allInventoryItems[i].DisplayedItem.Name
                Array.Sort(allInventoryItems, (x, y) => string.Compare(x.DisplayedItem.Name, y.DisplayedItem.Name));

                break;

            case 2:
                // sort allInventoryItems by allInventoryItems[i].DisplayedItem.Type
                Array.Sort(allInventoryItems, (x, y) => string.Compare(x.DisplayedItem.Type.ToString(), y.DisplayedItem.Type.ToString()));

                break;

            case 3:
                // sort allInventoryItems by allInventoryItems[i].DisplayedItem.Value
                Array.Sort(allInventoryItems, (x, y) => y.DisplayedItem.Value.CompareTo(x.DisplayedItem.Value));

                break;

            case 4:
                // sort allInventoryItems by allInventoryItems[i].DisplayedItem.Weight
                Array.Sort(allInventoryItems, (x, y) => y.DisplayedItem.Weight.CompareTo(x.DisplayedItem.Weight));

                break;

            case 5:
                // sort allInventoryItems by allInventoryItems[i].DisplayedItem.Rarity
                Array.Sort(allInventoryItems, (x, y) => y.DisplayedItem.Rarity.CompareTo(x.DisplayedItem.Rarity));

                break;
        }

        for (int i = 0; i < allInventoryItems.Length; i++)
        {
            sortedInventoryItems[i].transform.SetSiblingIndex(i);
        }

    }


    /// <summary>
    /// Displays the items in the inventory.
    /// </summary>
    /// <param name="inventory"></param>
    public void DisplayInventory(Inventory inventory)
    {
        ClearInventory();
        displayedInventory = inventory;
        allInventoryItems = new ItemDisplayer[inventory.inventory.Count];

        foreach (var item in inventory.inventory)
        {
            var itemDisplayer = Instantiate(itemDisplayerPrefab, itemsTransform).GetComponent<ItemDisplayer>();

            //itemDisplayer.eqipUseButton.onClick.AddListener(() => EquipUseItem(itemDisplayer.DisplayedItem));
            //itemDisplayer.dropButton.onClick.AddListener(() => DropItem(itemDisplayer.DisplayedItem));

            itemDisplayer.DisplayItem(item.Key, item.Value, ItemClickedd);
            allInventoryItems.SetValue(itemDisplayer, Array.IndexOf(allInventoryItems, null));
        }

        // Get the first item in inventory.inventory and display its info.
        if (allInventoryItems.Length > 0)
        {
            selectedItemInfoDisplayer.DisplayItemInfo(allInventoryItems[0].DisplayedItem, EquipUseItem, DropItem);
        }
        else // If there are no items in the inventory, display nothing.
        {
            selectedItemInfoDisplayer.DisplayItemInfo(null);
        }
    }


    /// <summary>
    /// Clears the inventory.
    /// </summary>
    public void ClearInventory()
    {
        foreach (Transform child in itemsTransform)
        {
            Destroy(child.gameObject);
        }
    }


    /// <summary>
    /// Called when an item is clicked.
    /// </summary>
    /// <param name="item"></param>
    private void ItemClickedd(Item item)
    {
        selectedItemInfoDisplayer.DisplayItemInfo(item, EquipUseItem, DropItem);
    }


    /// <summary>
    /// Equips or uses an item from the inventory.
    /// </summary>
    /// <param name="item"></param>
    private void EquipUseItem(Item item)
    {
        displayedInventory.EquipUseItem(item);
        DisplayInventory(displayedInventory);
    }


    /// <summary>
    /// Drops an item from the inventory.
    /// </summary>
    /// <param name="item"></param>
    private void DropItem(Item item)
    {
        // Get the index of the item that was dropped in allInventoryItems.
        int index = Array.IndexOf(allInventoryItems, allInventoryItems.Where(x => x.DisplayedItem == item).FirstOrDefault());

        displayedInventory.DropItem(item);
        DisplayInventory(displayedInventory);

        if (index < allInventoryItems.Length) // To avoid IndexOutOfRangeException.
        {
            selectedItemInfoDisplayer.DisplayItemInfo(allInventoryItems[index].DisplayedItem, EquipUseItem, DropItem);
        }
    }
}
