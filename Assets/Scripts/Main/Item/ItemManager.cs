using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : BaseManager<ItemManager>
{
    // 管理道具数量
    public Dictionary<ItemType, int> itemCounts = new Dictionary<ItemType, int>();

    // 判断目标物体状态
    public bool RemoveCollisionState = false;
    public bool SpeedUpState = false;

    // 管理道具种类
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
            Debug.LogWarning($"道具 {type} 数量不足");
            return;
        }

        if (itemDictionary.TryGetValue(type, out IItem item))
        {
            item.Use(target);
            itemCounts[type]--;
            // 更新 UI
        }
    }

    public void AddItem(ItemType type, int amount)
    {
        // 更新 UI
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
