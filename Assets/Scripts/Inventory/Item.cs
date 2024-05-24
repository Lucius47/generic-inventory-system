using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public GameObject itemPrefab;
    public Sprite itemSprite;

    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private ItemCategory itemType;
    [SerializeField] private int itemValue;
    [SerializeField] private int itemWeight;
    [SerializeField][Range(0, 5)] private int itemRarity;


    // Item Effects (Consumable)
    public int effectOnHealth;
    public int effectOnMana;
    public int effectOnStamina;

    // Item Effects (Equipment)
    [SerializeField] internal int itemDamage;
    [SerializeField] internal int itemAgility;

    //// Item Effects (Equipment)
    //public int effectOnStrength;
    //public int effectOnDexterity;
    //public int effectOnIntelligence;
    //public int effectOnWisdom;
    //public int effectOnCharisma;


    public string Name { get { return itemName; } }
    public string Description { get { return itemDescription; } }
    public int Value { get { return itemValue; } }
    public int Weight { get { return itemWeight; } }
    public int Rarity { get { return itemRarity; } }
    public ItemCategory Type { get { return itemType; } }

    public enum ItemCategory
    {
        Equipment,
        Consumable,
        Quest,
    }

    public void SetItem(GameObject itemPrefab, Sprite itemSprite, 
        string itemName, string itemDescription, ItemCategory itemType, 
        int itemValue, int itemWeight, int itemRarity, int effectOnHealth, 
        int effectOnMana, int effectOnStamina, int itemDamage, int itemAgility)
    {
        this.itemPrefab = itemPrefab;
        this.itemSprite = itemSprite;
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.itemType = itemType;
        this.itemValue = itemValue;
        this.itemWeight = itemWeight;
        this.itemRarity = itemRarity;
        this.effectOnHealth = effectOnHealth;
        this.effectOnMana = effectOnMana;
        this.effectOnStamina = effectOnStamina;
        this.itemDamage = itemDamage;
        this.itemAgility = itemAgility;
    }
}
