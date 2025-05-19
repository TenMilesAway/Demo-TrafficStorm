using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BonusType
{
    Normal,      // ��ͨ������ + 10 ��1%  ���ʻ�õ���
    Excellent,   // ���������� + 15 ��5%  ���ʻ�õ���
    Epic,        // ʷʫ������ + 20 ��10% ���ʻ�õ���
    Legend,      // ��˵������ + 50 ��30% ���ʻ�õ���
    Myth,        // �񻰣����� + 100��50% ���ʻ�õ���
}

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
            Debug.LogWarning($"���� {type} ��������");
            return;
        }

        if (itemDictionary.TryGetValue(type, out IItem item))
        {
            item.Use(target);
            itemCounts[type]--;
            // ���� UI
            EventCenter.GetInstance().EventTrigger("UpdateUI");
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

    /// <summary>
    /// ���ݸ��ʻ�ȡ����
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="pro"></param>
    public void AddItemByPro(int amount, float pro)
    {
        // ����һ���������Ϊ���εĸ���
        
        float random = UnityEngine.Random.Range(0f, 1f);
        Debug.Log(random);

        // ����һ���������Ϊ��ȡ���ߵ�����
        ItemType type = (ItemType)UnityEngine.Random.Range(0, 3);

        // �����С��������ʾͻ�ȡ
        if (random <= pro)
        {
            itemCounts[type] += amount;
            Debug.Log("��õ��� " + type.ToString());
            return;
        }

        Debug.Log("û�л�õ���");
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
