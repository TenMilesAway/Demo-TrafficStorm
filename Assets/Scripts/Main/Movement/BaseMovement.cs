using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMovement : MonoBehaviour
{
    public MovementType movementType;      // 运动类型枚举
    public List<Vector3> pathPoints;       // 路径点集合

    public float moveSpeed = 5f;           // 移动速度
    public float rotateSpeed = 360f;       // 旋转速度

    protected Vector3 moveDirection;       // 移动方向向量
    protected Quaternion targetRotation;   // 目标朝向

    protected int currentTargetIndex = 0;  // 当前目标点索引
    protected bool isTurning = false;      // 转向状态
    protected bool isMoving = false;       // 移动状态

    #region Unity 生命周期
    protected virtual void Awake()
    {

    }

    protected virtual void Update()
    {
        UpdateMovement();
    }
    #endregion

    #region Main Methods
    protected virtual void StartTurn(Vector3 targetPoint)
    {
        isTurning = true;
        Vector3 targetDirection = (targetPoint - transform.position).normalized;
        targetRotation = Quaternion.LookRotation(targetDirection);
    }

    protected virtual void UpdateMovement()
    {
        if (!isMoving) return;
        
        if (currentTargetIndex < pathPoints.Count)
        {
            Vector3 targetPoint = pathPoints[currentTargetIndex];
            // 到达当前路径点
            if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
            {
                currentTargetIndex++;
                // 到达以后，切换下个路径点
                if (currentTargetIndex < pathPoints.Count)
                {
                    targetPoint = pathPoints[currentTargetIndex];
                    moveDirection = (targetPoint - transform.position).normalized;
                    StartTurn(targetPoint);
                }
            }
            else
            {
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
                if (isTurning)
                {
                    // 平滑转向
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                    if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
                        isTurning = false;
                }
            }
        }
        else
        {
            isMoving = false;
        }
    }

    protected abstract void Initialize();
    #endregion
}
