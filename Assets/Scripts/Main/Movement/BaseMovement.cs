using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class BaseMovement : MonoBehaviour
{
    public MovementType movementType;          // 运动类型枚举

    public List<Vector3> pathPoints;           // 路径点集合

    public float moveSpeed = 5f;               // 移动速度
    public float rotateSpeed = 360f;           // 旋转速度
    public float enduranceTime = 3f;           // 倒计时

    protected Vector3 moveDirection;           // 移动方向向量
    protected Vector3 endurationLocalScale;    // 倒计时大小
    protected Quaternion targetRotation;       // 目标朝向

    protected Coroutine enduranceCoroutinue;   // 倒计时协程
    protected TMP_Text textEnduranceTime;      // 计时 TMP

    protected int currentTargetIndex = 0;      // 当前目标点索引
    protected bool isTurning = false;          // 转向状态
    protected bool isMoving = false;           // 移动状态

    [SerializeField] protected Vector3 enduranceTextOffset;

    #region Unity 生命周期
    protected virtual void Awake()
    {
        textEnduranceTime = GetComponentInChildren<TMP_Text>();
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
            PoolMgr.GetInstance().PushObj(gameObject.name.ToString(), gameObject);
            isMoving = false;
        }
    }

    public virtual void StartEnduranceTimer()
    {
        if (!this.isActiveAndEnabled) return;
        if (enduranceCoroutinue != null)
            StopCoroutine(enduranceCoroutinue);
        enduranceCoroutinue = StartCoroutine(EnduraceCountdown());
    }

    protected virtual void ResetEnduranceUI()
    {
        if (enduranceCoroutinue != null)
            StopCoroutine(enduranceCoroutinue);

        textEnduranceTime.text = "";
        textEnduranceTime.transform.localScale = endurationLocalScale;
        textEnduranceTime.transform.localPosition = enduranceTextOffset;
        textEnduranceTime.color = Color.white;
    }

    protected virtual IEnumerator EnduraceCountdown()
    {
        float startTime = Time.time;
        bool warningTriggered = false;
        Coroutine scaleCoroutine = null;

        while (Time.time - startTime < enduranceTime)
        {
            float remainingTime = enduranceTime - (Time.time - startTime);
            if (remainingTime <= 1f && !warningTriggered)
            {
                warningTriggered = true;
                textEnduranceTime.text = "!";
                scaleCoroutine = StartCoroutine(WarningScaleAnimation());
            }
            else if (remainingTime > 1 && remainingTime <= 3)
            {
                textEnduranceTime.text = Mathf.CeilToInt(remainingTime).ToString();
            }
            yield return null;
        }

        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }

        ChangeMoveState();
        ResetEnduranceUI();
    }

    protected virtual IEnumerator WarningScaleAnimation()
    {
        float duration = 1f;
        float elapsed = 0f;
        Vector3 maxScale = endurationLocalScale * 1.5f;
        textEnduranceTime.color = Color.red;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            textEnduranceTime.transform.localScale = Vector3.Lerp(endurationLocalScale, maxScale, Mathf.PingPong(t * 2f, 1f));
            elapsed += Time.deltaTime;
            yield return null;
        }
        textEnduranceTime.color = Color.white;
    }

    public virtual void ChangeMoveState()
    {
        isMoving = !isMoving;
        if (isMoving == false)
            StartEnduranceTimer();
        else
            ResetEnduranceUI();
    }

    protected abstract void Initialize();
    #endregion
}
