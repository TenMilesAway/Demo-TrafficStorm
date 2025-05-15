using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��·����������ͣ���г����ж�����ֹͣ 5 �복������
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
