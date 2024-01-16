using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayer : MonoBehaviour
{
    public Item DisplayedItem { get; private set; }

    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Type;
    [SerializeField] private TextMeshProUGUI Value;
    [SerializeField] private TextMeshProUGUI Weight;
    [SerializeField] private TextMeshProUGUI Rarity;
    [SerializeField] private Image Icon;

    [SerializeField] private TextMeshProUGUI quantity;

    //[SerializeField] private GameObject optionsMenu;
    //public Button eqipUseButton;
    //public Button dropButton;
    [SerializeField] private Button button;

    private void Awake()
    {
        button = GetComponent<Button>()/*.onClick.AddListener(OnClick)*/;
        //optionsMenu.SetActive(false);
    }

    //private void OnClick()
    //{
    //    if (DisplayedItem.Type == Item.ItemCategory.Quest) // No options for quest items. Cannot equip, use, or drop quest items.
    //    {
    //        return;
    //    }
    //    optionsMenu.SetActive(!optionsMenu.activeSelf);
    //}

    public void DisplayItem(Item item, int quantity, System.Action<Item> itemClicked = null)
    {
        DisplayedItem = item;

        Name.text = item.Name;
        Type.text = item.Type.ToString();
        Value.text = item.Value.ToString();
        Weight.text = item.Weight.ToString();
        Rarity.text = item.Rarity.ToString();
        Icon.sprite = item.itemSprite;

        this.quantity.text = quantity.ToString();
        if (quantity == 1)
        {
            this.quantity.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            this.quantity.transform.parent.gameObject.SetActive(true);
        }

        SetColorByRarity();

        if (itemClicked != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => itemClicked(item));
        }
    }

    private void SetColorByRarity()
    {
        var image = Rarity.transform.parent.GetComponent<Image>();
        // Rarity can be from 0 to 5. Set the color of the item name based on the rarity.
        switch (DisplayedItem.Rarity)
        {
            case 0:
                image.color = Color.grey;
                break;

            case 1:
                image.color = Color.green;
                break;

            case 2:
                image.color = Color.blue;
                break;

            case 3:
                image.color = Color.magenta;
                break;

            case 4:
                image.color = Color.yellow;
                break;
            case 5:
                image.color = new Color(1, 0.8431373f, 0); // gold
                break;
        }
    }
}
