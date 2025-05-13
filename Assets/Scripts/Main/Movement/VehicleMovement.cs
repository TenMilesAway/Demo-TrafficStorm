using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : BaseMovement
{
    public DirectionType startDirection;


    #region Unity 生命周期
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitializeVehicle(startDirection, movementType);
    }
    #endregion

    #region Init Methods
    public void InitializeVehicle(DirectionType directionType, MovementType moveType)
    {
        startDirection = directionType;
        movementType = moveType;
        Initialize();
    }

    protected override void Initialize()
    {
        currentTargetIndex = 0;
        isMoving = true; 
        pathPoints = VehiclePathPresetSystem.GetPath(startDirection, movementType);
        transform.position = pathPoints[0];

        // 设置初始旋转
        Vector3 initialDirection = (pathPoints[1] - pathPoints[0]).normalized;
        transform.rotation = Quaternion.LookRotation(initialDirection);

        // 同步转向状态
        targetRotation = transform.rotation;
        isTurning = false;
    }
    #endregion
}
