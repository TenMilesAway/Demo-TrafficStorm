﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mono 管理者
/// </summary>
public class MonoController : MonoBehaviour {

    private event UnityAction updateEvent;

	void Start () {
        DontDestroyOnLoad(this.gameObject);
	}
	
	void Update () {
        if (updateEvent != null)
            updateEvent();
    }

    /// <summary>
    /// 给外部提供添加帧更新事件的函数
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun;
    }

    /// <summary>
    /// 提供给外部用于移除帧更新事件函数
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        updateEvent -= fun;
    }
}
