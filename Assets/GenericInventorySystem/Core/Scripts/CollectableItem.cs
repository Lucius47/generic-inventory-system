using UnityEngine;

namespace GIS
{
    /// <summary>
    /// This class is used to represent an inventory item in the world. It holds a reference to its corresponding ScriptableObject
    /// </summary>
    public class CollectableItem : MonoBehaviour
    {
        [SerializeField] internal Item Item;

        public void SetItem(Item item)
        {
            if (this.Item == null)
            {
                this.Item = item;
            }
        }
    }
}