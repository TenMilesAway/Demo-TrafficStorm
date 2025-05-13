using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 工厂产品类接口
/// </summary>
public interface IProduct
{
    
}

/// <summary>
/// 工厂抽象类
/// 遵循原则：对拓展开放，对修改封闭
/// </summary>
public abstract class Factory : MonoBehaviour
{
    public static T GetRandomEnum<T>() where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }
}
