using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopPanel : BasePanel
{
    private Button btnContinue;
    private Button btnRestart;

    #region Unity ÉúÃüÖÜÆÚ
    protected override void Start()
    {
        
    }

    protected override void OnDestroy()
    {
        RemoveListeners();
    }
    #endregion

    #region Init Methods
    protected override void InitUI()
    {
        btnContinue = GetControl<Button>("btnContinue");
        btnRestart  = GetControl<Button>("btnRestart");

        AddListeners();
    }

    private void AddListeners()
    {
        btnContinue.onClick.AddListener(onContinue);
    }

    private void RemoveListeners()
    {

    }
    #endregion

    #region Main Methods
    private void onContinue()
    {
        UIManager.GetInstance().HidePanel("StopPanel");
        Time.timeScale = 1f;
    }
    #endregion
}
