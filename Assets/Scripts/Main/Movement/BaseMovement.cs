using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class BaseMovement : MonoBehaviour
{
    public MovementType movementType;          // �˶�����ö��

    public List<Vector3> pathPoints;           // ·���㼯��

    public float moveSpeed = 5f;               // �ƶ��ٶ�
    public float rotateSpeed = 360f;           // ��ת�ٶ�
    public float enduranceTime = 3f;           // ����ʱ

    protected Vector3 moveDirection;           // �ƶ���������
    protected Vector3 endurationLocalScale;    // ����ʱ��С
    protected Quaternion targetRotation;       // Ŀ�곯��

    protected Coroutine enduranceCoroutinue;   // ����ʱЭ��
    protected TMP_Text textEnduranceTime;      // ��ʱ TMP

    protected int currentTargetIndex = 0;      // ��ǰĿ�������
    protected bool isTurning = false;          // ת��״̬
    protected bool isMoving = false;           // �ƶ�״̬

    [SerializeField] protected Vector3 enduranceTextOffset;

    #region Unity ��������
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
