﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 不能保证唯一性
public class SingletonMono<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        return instance;
    }

    protected virtual void Awake()
    {
        instance = this as T;
    }
	
}
