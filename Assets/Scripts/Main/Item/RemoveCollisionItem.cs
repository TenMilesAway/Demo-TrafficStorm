using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 碰撞移除卡：移除指定实体的碰撞，并在下次生成时重置碰撞
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
