using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : BaseMovement
{
    public DirectionType startDirection;

    private SpriteRenderer arrowSprite;

    #region Unity ��������
    protected override void Awake()
    {
        base.Awake();
        arrowSprite = GetComponentInChildren<SpriteRenderer>();
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
        // ��ʼ��״̬
        currentTargetIndex = 0;
        isMoving = true; 
        pathPoints = VehiclePathPresetSystem.GetPath(startDirection, movementType);
        transform.position = pathPoints[0];
        arrowSprite.sprite = GetArrowSprite(movementType);
        speedMultiplier = 1f;

        // ���ó�ʼ��ת
        Vector3 initialDirection = (pathPoints[1] - pathPoints[0]).normalized;
        transform.rotation = Quaternion.LookRotation(initialDirection);
        transform.localScale = Vector3.one;
        endurationLocalScale = Vector3.one;

        // ͬ��ת��״̬
        targetRotation = transform.rotation;
        isTurning = false;

        // ���õ���ʱ��ʾ
        ResetEnduranceUI();

        // ������ײ��Ч��
        GetComponentInChildren<Collider>().enabled = true;
    }
    #endregion

    #region Main Methods
    private Sprite GetArrowSprite(MovementType moveType)
    {
        string arrowSpritePath = "";

        switch (moveType)
        {
            case MovementType.Straight:
                arrowSpritePath = "Sprites/StraightArrow";
                break;
            case MovementType.LeftTurn:
                arrowSpritePath = "Sprites/LeftTurnArrow";
                break;
            case MovementType.RightTurn:
                arrowSpritePath = "Sprites/RightTurnArrow";
                break;
        }

        return ResMgr.GetInstance().Load<Sprite>(arrowSpritePath);
    }
    #endregion
}
