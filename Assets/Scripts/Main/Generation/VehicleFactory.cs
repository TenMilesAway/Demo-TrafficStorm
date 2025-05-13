using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VehicleFactory : Factory
{
    public static void CreateProduct(string name)
    {
        PoolMgr.GetInstance().GetObj(name, (o) => 
        {
            VehicleMovement vehicle = o.GetComponent<VehicleMovement>();
            DirectionType randomDir = GetRandomEnum<DirectionType>();
            MovementType randomMove = GetRandomEnum<MovementType>();
            vehicle.InitializeVehicle(randomDir, randomMove);
        });
    }
}
