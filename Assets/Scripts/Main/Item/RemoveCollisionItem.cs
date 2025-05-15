using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ײ�Ƴ������Ƴ�ָ��ʵ�����ײ�������´�����ʱ������ײ
/// </summary>
public class RemoveCollisionItem : IItem
{
    public ItemType Type => ItemType.RemoveCollision;

    public void Use(GameObject target)
    {
        Collider collider = target.GetComponentInChildren<Collider>();
        collider.enabled = false;
        target.GetComponent<BaseMovement>().ChangeToMoving();
        ItemManager.GetInstance().RemoveCollisionState = false;
    }
}
