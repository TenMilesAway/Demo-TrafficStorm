using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BonusType
{
    Normal,      // 普通，积分 + 10 ，1%  概率获得道具
    Excellent,   // 精良，积分 + 15 ，5%  概率获得道具
    Epic,        // 史诗，积分 + 20 ，10% 概率获得道具
    Legend,      // 传说，积分 + 50 ，30% 概率获得道具
    Myth,        // 神话，积分 + 100，50% 概率获得道具
}

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
        itemCounts[ItemType.RemoveCollision] = 2;
        itemCounts[ItemType.RoadBlock] = 2;
        itemCounts[ItemType.SpeedUp] = 2;
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
            EventCenter.GetInstance().EventTrigger("UpdateUI");
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

    /// <summary>
    /// 根据概率获取道具
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="pro"></param>
    public void AddItemByPro(int amount, float pro)
    {
        // 生成一个随机数作为本次的概率
        
        float random = UnityEngine.Random.Range(0f, 1f);
        Debug.Log(random);

        // 生成一个随机数作为获取道具的种类
        ItemType type = (ItemType)UnityEngine.Random.Range(0, 3);

        // 随机数小于这个概率就获取
        if (random <= pro)
        {
            itemCounts[type] += amount;
            Debug.Log("获得道具 " + type.ToString());
            return;
        }

        Debug.Log("没有获得道具");
    }

    public int GetItemAmount(ItemType type)
    {
        if (itemCounts.ContainsKey(type))
            return itemCounts[type];

        return 0;
    }

    public void ResetAllStates()
    {
        RemoveCollisionState = false;
        SpeedUpState = false;
    }
    #endregion
}
