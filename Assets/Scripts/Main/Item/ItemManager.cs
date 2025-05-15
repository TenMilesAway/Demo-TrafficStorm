using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : BaseManager<ItemManager>
{
    // �����������
    public Dictionary<ItemType, int> itemCounts = new Dictionary<ItemType, int>();

    // �ж�Ŀ������״̬
    public bool RemoveCollisionState = false;
    public bool SpeedUpState = false;

    // �����������
    private Dictionary<ItemType, IItem> itemDictionary = new Dictionary<ItemType, IItem>();

    #region Init Methods
    public void InitData()
    {
        itemDictionary[ItemType.RemoveCollision] = new RemoveCollisionItem();
        itemDictionary[ItemType.RoadBlock] = new RoadBlockItem();
        itemDictionary[ItemType.SpeedUp] = new SpeedUpItem();
        itemCounts[ItemType.RemoveCollision] = 1;
        itemCounts[ItemType.RoadBlock] = 1;
        itemCounts[ItemType.SpeedUp] = 1;
    }
    #endregion

    #region Main Methods
    public void UseItem(ItemType type, GameObject target)
    {
        if (!itemCounts.ContainsKey(type) || itemCounts[type] <= 0)
        {
            Debug.LogWarning($"���� {type} ��������");
            return;
        }

        if (itemDictionary.TryGetValue(type, out IItem item))
        {
            item.Use(target);
            itemCounts[type]--;
            // ���� UI
        }
    }

    public void AddItem(ItemType type, int amount)
    {
        // ���� UI
        if (itemCounts.ContainsKey(type))
        {
            itemCounts[type] += amount;
        }
        else
        {
            itemCounts[type] = amount;
        }
    }

    public int GetItemAmount(ItemType type)
    {
        if (itemCounts.ContainsKey(type))
            return itemCounts[type];

        return 0;
    }
    #endregion
}
