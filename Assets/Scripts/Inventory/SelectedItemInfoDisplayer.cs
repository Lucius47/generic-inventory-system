using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectedItemInfoDisplayer : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemHealthText;
    [SerializeField] private TextMeshProUGUI itemManaText;
    [SerializeField] private TextMeshProUGUI itemStaminaText;
    [SerializeField] private TextMeshProUGUI itemDamageText;
    [SerializeField] private TextMeshProUGUI itemAgilityText;

    [SerializeField] internal Button equipUseButton;
    [SerializeField] internal Button dropButton;

    internal void DisplayItemInfo(Item item, System.Action<Item> equipUseItem = null, System.Action<Item> dropItem = null)
    {
        if (item != null)
        {
            itemImage.sprite = item.itemSprite;
            itemNameText.text = item.Name;
            itemDescriptionText.text = item.Description;
            itemTypeText.text = item.Type.ToString();
            
            itemHealthText.text = "Health +" + item.effectOnHealth.ToString();
            itemManaText.text = "Mana +" + item.effectOnMana.ToString();
            itemStaminaText.text = "Stamina +" + item.effectOnStamina.ToString();
            itemDamageText.text = "Damage " + item.itemDamage.ToString();
            itemAgilityText.text = "Agility " + item.itemAgility.ToString();

            if (equipUseItem != null)
            {
                equipUseButton.onClick.RemoveAllListeners();
                equipUseButton.onClick.AddListener(delegate { equipUseItem(item); });
            }
            if (dropItem != null)
            {
                dropButton.onClick.RemoveAllListeners();
                dropButton.onClick.AddListener(delegate { dropItem(item); });
            }

            itemHealthText.gameObject.SetActive(item.effectOnHealth != 0);
            itemManaText.gameObject.SetActive(item.effectOnMana != 0);
            itemStaminaText.gameObject.SetActive(item.effectOnStamina != 0);
            itemDamageText.gameObject.SetActive(item.itemDamage != 0);
            itemAgilityText.gameObject.SetActive(item.itemAgility != 0);

            if (item.Type == Item.ItemCategory.Quest)
            {
                equipUseButton.gameObject.SetActive(false);
                dropButton.gameObject.SetActive(false);
            }
            else
            {
                equipUseButton.gameObject.SetActive(true);
                dropButton.gameObject.SetActive(true);
            }
        }
        else // Show placeholder info.
        {
            itemImage.gameObject.SetActive(false);
            itemNameText.gameObject.SetActive(false);
            itemDescriptionText.gameObject.SetActive(false);
            itemTypeText.gameObject.SetActive(false);
            itemHealthText.gameObject.SetActive(false);
            itemManaText.gameObject.SetActive(false);
            itemStaminaText.gameObject.SetActive(false);
            itemDamageText.gameObject.SetActive(false);
            itemAgilityText.gameObject.SetActive(false);

            dropButton.gameObject.SetActive(false);
            dropButton.gameObject.SetActive(false);
        }
    }
}
