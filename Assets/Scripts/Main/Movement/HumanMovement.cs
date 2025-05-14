using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMovement : BaseMovement
{
    public DirectionHumanType startDirection;

    private Animator animator;

    #region Unity ÉúÃüÖÜÆÚ
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
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
        transform.localScale = new Vector3(2.1f, 2.1f, 2.1f);
        endurationLocalScale = new Vector3(0.4f, 0.4f, 0.4f);
        ResetEnduranceUI();
    }

    public override void ChangeMoveState()
    {
        base.ChangeMoveState();
        if (isMoving)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);
    }
    #endregion
}
