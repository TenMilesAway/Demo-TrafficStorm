using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Start()
    {
        UIManager.GetInstance().ShowPanel<StartPanel>("StartPanel");
    }

    void Update()
    {
        
    }
}
