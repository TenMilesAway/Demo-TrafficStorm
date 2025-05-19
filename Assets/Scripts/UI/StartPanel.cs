using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
    private TMP_Text textTMPGameName;
    private Button btnStart;
    private Button btnRank;
    private Button btnShop;

    #region Unity 生命周期
    protected override void Start()
    {
        InitUI();
        InitListeners();

        MusicMgr.GetInstance().PlayBkMusic("cake");
    }

    protected override void OnDestroy()
    {
        RemoveListeners();
    }
    #endregion

    #region Init Methods
    protected override void InitUI()
    {
        textTMPGameName = GetControl<TMP_Text>("textTMPGameName");
        btnStart = GetControl<Button>("btnStart");
        btnRank = GetControl<Button>("btnRank");
        btnShop = GetControl<Button>("btnShop");
    }

    private void InitListeners()
    {
        btnStart.onClick.AddListener(OnStart);
    }

    private void RemoveListeners()
    {
        btnStart.onClick.RemoveListener(OnStart);
    }
    #endregion

    #region Listen Methods
    public void OnStart()
    {
        UIManager.GetInstance().HidePanel("StartPanel");
        UIManager.GetInstance().ShowPanel<MainPanel>("MainPanel");

        MusicMgr.GetInstance().PlaySound("buttonOn", false);
        MusicMgr.GetInstance().PlayBkMusic("one");

        // 初始化
        ItemManager.GetInstance().InitData();
        GeneratePool.GetInstance().InitData();
    }
    #endregion
}
