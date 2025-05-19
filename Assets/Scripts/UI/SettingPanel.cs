using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    private Button btnClose;

    #region Unity ÉúÃüÖÜÆÚ
    protected override void Start()
    {
        InitUI();
    }

    protected override void OnDestroy()
    {
        RemoveListeners();
    }
    #endregion

    #region Init Methods
    protected override void InitUI()
    {
        btnClose = GetControl<Button>("btnClose");

        AddListeners();
    }

    private void AddListeners()
    {
        btnClose.onClick.AddListener(OnClose);
    }

    private void RemoveListeners()
    {

    }
    #endregion

    #region Main Methods
    private void OnClose()
    {
        UIManager.GetInstance().HidePanel("SettingPanel");
        Time.timeScale = 1f;
    }
    #endregion
}
