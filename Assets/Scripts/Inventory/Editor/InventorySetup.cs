using UnityEngine;
using UnityEditor;
using static Item;

public class InventorySetup : MonoBehaviour
{
    [MenuItem("Inventory System/Turn Object Into Inventory Item")]
    public static void TurnObjectIntoInventoryItem()
    {
        GameObject obj = Selection.activeGameObject;

        if (obj != null)
        {
            InventoryItemEditorWindow window =
            (InventoryItemEditorWindow)EditorWindow.GetWindow(typeof(InventoryItemEditorWindow));

            window.itemObject = obj;
            window.createFromObj = true;

            window.Show();
        }
    }

    [MenuItem("Inventory System/Add Item")]
    public static void AddItem()
    {
        InventoryItemEditorWindow window = 
            (InventoryItemEditorWindow)EditorWindow.GetWindow(typeof(InventoryItemEditorWindow));
        window.Show();
    }


    [MenuItem("Inventory System/Add Inventory")]
    public static void AddInventory()
    {
        GameObject obj = Selection.activeGameObject;
        if (obj != null)
        {
            obj.AddComponent<Inventory>();
        }
    }
}

internal class InventoryItemEditorWindow : EditorWindow
{
    public InventoryItemEditorWindow()
    {
        titleContent = new GUIContent("Inventory Item Editor");
        minSize = new Vector2(300, 550);
    }

    internal bool createFromObj = false;
    internal GameObject itemObject;

    private string itemName = "Item Name";
    private string itemDescription = "Description";
    private Sprite itemIcon;
    private ItemCategory itemType;
    private int itemValue;
    private int itemWeight;
    [Range(0, 5)] private int itemRarity;
    private int effectOnHealth;
    private int effectOnMana;
    private int effectOnStamina;
    private int itemDamage;
    private int itemAgility;

    private string errorMessage = "";

    private void OnGUI()
    {
        if(createFromObj) GUI.enabled = false;
        GUILayout.Label("Item Prefab*");
        itemObject = EditorGUILayout.ObjectField(itemObject, typeof(GameObject), false) as GameObject;

        GUI.enabled = true;
        GUILayout.Label("Item Name*");
        itemName = EditorGUILayout.TextField(itemName);

        GUILayout.Label("Item Description");
        itemDescription = EditorGUILayout.TextField(itemDescription);

        GUILayout.Label("Item Icon*");
        itemIcon = (Sprite)EditorGUILayout.ObjectField(itemIcon, typeof(Sprite), false);

        GUILayout.Label("Item Type");
        itemType = (ItemCategory)EditorGUILayout.EnumPopup(itemType);

        GUILayout.Label("Item Value");
        itemValue = EditorGUILayout.IntField(itemValue);

        GUILayout.Label("Item Weight");
        itemWeight = EditorGUILayout.IntField(itemWeight);

        GUILayout.Label("Item Rarity");
        itemRarity = EditorGUILayout.IntSlider(itemRarity, 0, 5);

        GUILayout.Label("Effect on Health");
        effectOnHealth = EditorGUILayout.IntField(effectOnHealth);

        GUILayout.Label("Effect on Mana");
        effectOnMana = EditorGUILayout.IntField(effectOnMana);

        GUILayout.Label("Effect on Stamina");
        effectOnStamina = EditorGUILayout.IntField(effectOnStamina);

        GUILayout.Label("Item Damage");
        itemDamage = EditorGUILayout.IntField(itemDamage);

        GUILayout.Label("Item Agility");
        itemAgility = EditorGUILayout.IntField(itemAgility);

        if (errorMessage != "")
        {
            GUI.color = Color.red;
        }
        GUILayout.Label(errorMessage);
        GUI.color = Color.white;


        if (GUILayout.Button("Save"))
        {
            if (itemObject == null)
            {
                errorMessage = "Item prefab cannot be empty";
                return;
            }
            if (itemName == "")
            {
                errorMessage = "Item name cannot be empty";
                return;
            }
            if (itemIcon == null)
            {
                errorMessage = "Item icon cannot be empty";
                return;
            }

            if (!itemObject.GetComponent<CollectableItem>())
            {
                itemObject.AddComponent<CollectableItem>();
            }

            if (itemObject.GetComponent<Collider>() == null)
            {
                itemObject.AddComponent<BoxCollider>();
                Debug.LogWarning("No collider found on the item object. Adding a BoxCollider");
            }


            if (!AssetDatabase.IsValidFolder("Assets/InventoryItems"))
            {
                AssetDatabase.CreateFolder("Assets", "InventoryItems");
            }

            if (createFromObj)
            {
                itemObject = PrefabUtility.SaveAsPrefabAssetAndConnect(
                    itemObject, "Assets/InventoryItems/" + itemObject.name + ".prefab", InteractionMode.UserAction);
            }


            Item item = ScriptableObject.CreateInstance<Item>();
            item.SetItem(itemObject, itemIcon, itemName, itemDescription, 
                itemType, itemValue, itemWeight, itemRarity, effectOnHealth, 
                effectOnMana, effectOnStamina, itemDamage, itemAgility);

            itemObject.GetComponent<CollectableItem>().Item = item;

            itemName = itemName.Trim();

            AssetDatabase.CreateAsset(item, "Assets/InventoryItems/" + itemName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Close();
        }
    }
}