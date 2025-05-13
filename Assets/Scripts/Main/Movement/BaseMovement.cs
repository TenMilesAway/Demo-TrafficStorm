using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMovement : MonoBehaviour
{
    public MovementType movementType;      // �˶�����ö��
    public List<Vector3> pathPoints;       // ·���㼯��

    public float moveSpeed = 5f;           // �ƶ��ٶ�
    public float rotateSpeed = 360f;       // ��ת�ٶ�

    protected Vector3 moveDirection;       // �ƶ���������
    protected Quaternion targetRotation;   // Ŀ�곯��

    protected int currentTargetIndex = 0;  // ��ǰĿ�������
    protected bool isTurning = false;      // ת��״̬
    protected bool isMoving = false;       // �ƶ�״̬

    #region Unity ��������
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
            // ���ﵱǰ·����
            if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
            {
                currentTargetIndex++;
                // �����Ժ��л��¸�·����
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
                    // ƽ��ת��
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
