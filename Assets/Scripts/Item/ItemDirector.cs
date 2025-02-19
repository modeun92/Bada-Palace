using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Item
{
    [CreateAssetMenu(fileName = "Data", menuName = "Scriptable Object/" + nameof(ItemDirector), order = 1)]
    public class ItemDirector : ScriptableObject
    {
        [SerializeField]
        private ItemForSale[] Items;
        public ItemForSale[] AllItems { get => (ItemForSale[])Items.Clone(); }
    }
}
