using UnityEngine;
using GIS.UI;

namespace Demo
{
    /// <summary>
    /// This class is used to manage the game.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public Character selectedPlayer; // Currently selected player
                                         // Multiple players are supported.
                                         // If another player/character is selected, their inventory will be displayed.

        [SerializeField] private InventoryDisplayer inventoryDisplayer; // Inventory displayer present in the scene

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            inventoryDisplayer.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I)) // Press I to toggle Inventory
            {
                if (inventoryDisplayer.gameObject.activeSelf) // If inventory is already active, close it
                {
                    inventoryDisplayer.gameObject.SetActive(false);
                    inventoryDisplayer.ClearInventory();
                    return;
                }

                inventoryDisplayer.gameObject.SetActive(true);
                inventoryDisplayer.DisplayInventory(selectedPlayer.inventory, selectedPlayer.EquipItem, selectedPlayer.UseItem, selectedPlayer.DropItem);
            }
        }

        /// <summary>
        /// This method is used to update the inventory.
        /// </summary>
        public void UpdateInventory()
        {
            inventoryDisplayer.DisplayInventory(selectedPlayer.inventory);
        }
    }
}
