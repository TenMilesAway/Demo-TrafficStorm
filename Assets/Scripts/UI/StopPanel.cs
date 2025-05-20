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

        AddListeners();
    }

    private void AddListeners()
    {
        btnContinue.onClick.AddListener(OnContinue);
    }

    private void RemoveListeners()
    {
        btnContinue.onClick.RemoveListener(OnContinue);
    }
    #endregion

    #region Main Methods
    private void OnContinue()
    {
        UIManager.GetInstance().HidePanel("StopPanel");
        Time.timeScale = 1f;
    }

    private void OnRestart()
    {
        UIManager.GetInstance().GetPanel<MainPanel>("MainPanel").DestoryAll();

        UIManager.GetInstance().HidePanel("StopPanel");
        UIManager.GetInstance().HidePanel("MainPanel");
        UIManager.GetInstance().ShowPanel<MainPanel>("MainPanel");
        
        MusicMgr.GetInstance().PlaySound("buttonOn", false);
    }
    #endregion
}
