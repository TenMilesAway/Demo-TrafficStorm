using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : BaseMovement
{
    public DirectionType startDirection;

    private SpriteRenderer arrowSprite;

    #region Unity 生命周期
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
        // 初始化状态
        currentTargetIndex = 0;
        isMoving = true; 
        pathPoints = VehiclePathPresetSystem.GetPath(startDirection, movementType);
        transform.position = pathPoints[0];
        arrowSprite.sprite = GetArrowSprite(movementType);
        speedMultiplier = 1f;

        // 设置初始旋转
        Vector3 initialDirection = (pathPoints[1] - pathPoints[0]).normalized;
        transform.rotation = Quaternion.LookRotation(initialDirection);
        transform.localScale = Vector3.one;
        endurationLocalScale = Vector3.one;

        // 同步转向状态
        targetRotation = transform.rotation;
        isTurning = false;

        // 重置倒计时提示
        ResetEnduranceUI();

        // 重置碰撞卡效果
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
