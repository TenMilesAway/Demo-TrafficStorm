using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    RemoveCollision, // 碰撞消除卡
    RoadBlock,       // 道路封路卡
    SpeedUp,         // 加速卡
    //PoliceCar,       // 警车召唤卡
}

public interface IItem
{
    ItemType Type { get; }
    void Use(GameObject target);
}
