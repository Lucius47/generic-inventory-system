using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Inventory))]
public class Character : MonoBehaviour
{
    public string characterName = "player1";

    public Inventory inventory;

    public int health = 10;
    public int mana = 10;
    public int stamina = 10;

    private readonly int maxHealth = 100;
    private readonly int maxMana = 100;
    private readonly int maxStamina = 100;

    [SerializeField] private Image healthBar;
    [SerializeField] private Image manaBar;
    [SerializeField] private Image staminaBar;

    [SerializeField] private Transform equipmentParent;

    (Item, GameObject) equippedItem;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        inventory.LoadInventory();
    }

    private void Start()
    {
        UpdateBars();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CollectableItem item))
        {
            inventory.AddItem(item.Item);
            Destroy(other.gameObject);
            GameManager.Instance.UpdateInventory();
        }
    }

    public void EquipItem(Item item)
    {
        if (equippedItem.Item2 != null)
        {
            Destroy(equippedItem.Item2);
        }
        equippedItem.Item1 = item;
        equippedItem.Item2 = Instantiate(item.itemPrefab, equipmentParent.position, equipmentParent.rotation, equipmentParent);
    }

    public void UseItem(Item item)
    {
        health += item.effectOnHealth;
        mana += item.effectOnMana;
        stamina += item.effectOnStamina;

        if (health > maxHealth) health = maxHealth;
        if (mana > maxMana) mana = maxMana;
        if (stamina > maxStamina) stamina = maxStamina;

        UpdateBars();
    }

    internal void DropItem(Item item)
    {
        if (equippedItem.Item2 != null)
        {
            if (equippedItem.Item1 == item)
            {
                equippedItem.Item1 = null;
                Destroy(equippedItem.Item2);
            }
        }

        Vector3 randomPos = (transform.position - Vector3.forward * 3) + UnityEngine.Random.insideUnitSphere;
        randomPos.y = transform.position.y;
        Instantiate(item.itemPrefab, randomPos, Quaternion.identity);
    }

    private void UpdateBars()
    {
        healthBar.fillAmount = (float)health / maxHealth;
        manaBar.fillAmount = (float)mana / maxMana;
        staminaBar.fillAmount = (float)stamina / maxStamina;
    }

    private void OnApplicationQuit()
    {
        inventory.SaveInventory();
    }
}
