using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMovement : BaseMovement
{
    public DirectionHumanType startDirection;

    #region Unity ÉúÃüÖÜÆÚ
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitializeHuman(startDirection, movementType);
    }
    #endregion

    #region Init Methods
    public void InitializeHuman(DirectionHumanType directionType, MovementType moveType)
    {
        startDirection = directionType;
        movementType = moveType;
        Initialize();
    }

    protected override void Initialize()
    {
        currentTargetIndex = 0;
        isMoving = true;
        pathPoints = HumanPathPresetSystem.GetPath(startDirection, movementType);
        transform.position = pathPoints[0];
    }
    #endregion
}
