using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpItem : IItem
{
    public ItemType Type => ItemType.SpeedUp;

    private float speedBoostMultiplier = 3f;
    private float boostDuration = 2f;

    public void Use(GameObject target)
    {
        BaseMovement baseMovement = target.GetComponent<BaseMovement>();
        baseMovement.ChangeToMoving();

        baseMovement.ApplySpeedBoost(speedBoostMultiplier, boostDuration);
        ItemManager.GetInstance().SpeedUpState = false;
    }
}
