using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 道路封锁卡：暂停所有车辆行动，并停止 5 秒车辆生成
/// </summary>
public class RoadBlockItem : IItem
{
    public ItemType Type => ItemType.RoadBlock;

    private float stopSeconds = 5f;

    public void Use(GameObject target)
    {
        VehicleMovement[] vehicles = Object.FindObjectsOfType<VehicleMovement>();
        foreach (VehicleMovement vehicle in vehicles)
        {
            vehicle.ChangeToIdle();
            vehicle.StartEnduranceTimer();
        }

        GeneratePool pool = Object.FindObjectOfType<GeneratePool>();
        pool.StopGenerateForSeconds(stopSeconds);
    }
}
