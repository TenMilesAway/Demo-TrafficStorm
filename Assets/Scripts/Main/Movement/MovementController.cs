using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovementController : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private float scaleMultiplier = 1.2f;
    [SerializeField] private float animationDuration = 0.2f;

    private Camera mainCamera;

    #region Unity ��������
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }
    #endregion

    #region Main Methods
    private void HandleClick()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 100f))
            return;

        BaseMovement target = hit.collider.GetComponentInParent<BaseMovement>();
        AnimateClick(target.transform);

        // ��ײ���߿�
        if (ItemManager.GetInstance().RemoveCollisionState)
        {
            ItemManager.GetInstance().UseItem(ItemType.RemoveCollision, target.gameObject);
            ItemManager.GetInstance().RemoveCollisionState = false;
            target.ResetEnduranceUI();
        }
        // ���ٵ��߿�
        else if (ItemManager.GetInstance().SpeedUpState)
        {
            ItemManager.GetInstance().UseItem(ItemType.SpeedUp, target.gameObject);
            ItemManager.GetInstance().SpeedUpState = false;
            target.ResetEnduranceUI();
        }
        // ��ͨ���
        else
        {
            target.ChangeMoveState();
        }

        MusicMgr.GetInstance().PlaySound("click", false);

        EventCenter.GetInstance().EventTrigger("ResetSelectedItem");
    }

    private void AnimateClick(Transform target)
    {
        StartCoroutine(ScaleAnimation(target));
    }

    IEnumerator ScaleAnimation(Transform target)
    {
        Vector3 originalScale = target.localScale;
        float halfDuration = animationDuration / 2f;

        // �Ŵ�
        yield return ScaleTo(target, originalScale * scaleMultiplier, halfDuration);
        // ��С
        yield return ScaleTo(target, originalScale, halfDuration);
    }

    IEnumerator ScaleTo(Transform target, Vector3 targetScale, float duration)
    {
        float elapsed = 0f;
        Vector3 startScale = target.localScale;

        while (elapsed < duration)
        {
            if (target.gameObject.activeInHierarchy == false)
                yield break;
            target.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.localScale = targetScale;
    }
    #endregion
}
