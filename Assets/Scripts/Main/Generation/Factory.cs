using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ������Ʒ��ӿ�
/// </summary>
public interface IProduct
{
    
}

/// <summary>
/// ����������
/// ��ѭԭ�򣺶���չ���ţ����޸ķ��
/// </summary>
public abstract class Factory : MonoBehaviour
{
    public static T GetRandomEnum<T>() where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }
}
