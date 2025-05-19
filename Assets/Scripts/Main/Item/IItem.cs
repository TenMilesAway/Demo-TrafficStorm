using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    RemoveCollision, // ��ײ������
    RoadBlock,       // ��·��·��
    SpeedUp,         // ���ٿ�
    //PoliceCar,       // �����ٻ���
}

public interface IItem
{
    ItemType Type { get; }
    void Use(GameObject target);
}
