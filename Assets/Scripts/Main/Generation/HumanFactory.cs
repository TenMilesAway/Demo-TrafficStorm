using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanFactory : Factory
{
    public static void CreateProduct(string name)
    {
        PoolMgr.GetInstance().GetObj(name, (o) =>
        {
            HumanMovement human = o.GetComponent<HumanMovement>();
            DirectionHumanType randomDir = GetRandomEnum<DirectionHumanType>();
            MovementType randomMove = GetRandomEnum<MovementType>();
            human.InitializeHuman(randomDir, randomMove);
        });
    }
}
