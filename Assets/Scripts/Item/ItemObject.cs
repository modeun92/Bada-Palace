using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Item
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField]
        private ItemDirector ItemDirector;
        [SerializeField]
        private ItemType m_ItemType;
        [SerializeField]
        private ItemForInventory m_Item;
        // Start is called before the first frame update
        void Start()
        {
            //m_Item = ItemDirector.IssueItem(m_ItemType);
        }

        // Update is called once per frame
        void Update()
        {
            gameObject.SetActive(!(m_Item?.Amount == 0));
        }
    }
}