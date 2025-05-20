using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class StartPanel : BasePanel
{
    private TextMeshProUGUI textTMPGameName;
    private Button btnStart;
    private Button btnRank;
    private Button btnSetting;

    private Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow };

    #region Unity ÉúÃüÖÜÆÚ
    protected override void Start()
    {
        InitUI();
        AddListeners();

        StartCoroutine(TMPRoutine());

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
        textTMPGameName = GetControl<TextMeshProUGUI>("textTMPGameName");
        btnStart   = GetControl<Button>("btnStart");
        btnRank    = GetControl<Button>("btnRank");
        btnSetting = GetControl<Button>("btnSetting");
    }

    private void AddListeners()
    {
        btnStart.onClick.AddListener(OnStart);
        btnSetting.onClick.AddListener(OnSetting);
    }

    private void RemoveListeners()
    {
        btnStart.onClick.RemoveListener(OnStart);
        btnSetting.onClick.RemoveListener(OnSetting);
    }
    #endregion

    #region Main Methods
    private void OnStart()
    {
        UIManager.GetInstance().HidePanel("StartPanel");
        UIManager.GetInstance().ShowPanel<MainPanel>("MainPanel");

        MusicMgr.GetInstance().PlaySound("buttonOn", false);
    }

    private void OnSetting()
    {
        UIManager.GetInstance().ShowPanel<SettingPanel>("SettingPanel", E_UI_Layer.Top, (panel) => 
        {
            panel.SetMusicValue(MusicMgr.GetInstance().GetBKValue() * 100);
            panel.SetSFXValue(MusicMgr.GetInstance().GetSFXValue() * 100);
        });

        MusicMgr.GetInstance().PlaySound("buttonOn", false);
    }

    private IEnumerator TMPRoutine()
    {
        int index = 0;
        float targetFade = 0.8f;
        float originFade = 1.0f;
        float duration = 3f;

        while (true)
        {
            textTMPGameName.DOColor(colors[index], duration);

            Sequence sequence = DOTween.Sequence();

            sequence.Append(textTMPGameName.DOFade(targetFade, duration / 2));
            sequence.Append(textTMPGameName.DOFade(originFade, duration / 2));


            index++;
            index %= colors.Length;

            yield return new WaitForSeconds(duration);
        }
    }
    #endregion
}
