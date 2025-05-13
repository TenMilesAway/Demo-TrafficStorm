using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : BaseMovement
{
    public DirectionType startDirection;


    #region Unity ��������
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

        // ���ó�ʼ��ת
        Vector3 initialDirection = (pathPoints[1] - pathPoints[0]).normalized;
        transform.rotation = Quaternion.LookRotation(initialDirection);

        // ͬ��ת��״̬
        targetRotation = transform.rotation;
        isTurning = false;
    }
    #endregion
}
