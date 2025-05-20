using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverPanel : BasePanel
{
    private Button btnBack;
    private Button btnRestart;

    private TextMeshProUGUI textScore;

    private bool isListenersAlready = false;

    #region Unity ��������
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
        btnBack = GetControl<Button>("btnBack");
        btnRestart = GetControl<Button>("btnRestart");

        textScore = GetControl<TextMeshProUGUI>("textScore");

        AddListeners();
    }

    private void AddListeners()
    {
        // ��ע��
        // ���������Ϊ�˲��ظ���Ӽ���
        // �����ǵ��������� Add�����Ǿ��岻�������������Ų�
        if (!isListenersAlready)
        {
            isListenersAlready = true;
            btnBack.onClick.AddListener(OnBackStart);
            btnRestart.onClick.AddListener(OnRestart);
        }
    }

    private void RemoveListeners()
    {
        btnBack.onClick.RemoveListener(OnBackStart);
        btnRestart.onClick.RemoveListener(OnRestart);
    }
    #endregion

    #region Main Methods
    private void OnBackStart()
    {
        btnBack.image.color = Color.white;
        UIManager.GetInstance().HidePanel("GameOverPanel");
        UIManager.GetInstance().HidePanel("MainPanel");
        UIManager.GetInstance().ShowPanel<StartPanel>("StartPanel");

        MusicMgr.GetInstance().PlaySound("buttonOn", false);
    }

    private void OnRestart()
    {
        UIManager.GetInstance().HidePanel("GameOverPanel");
        UIManager.GetInstance().HidePanel("MainPanel");
        UIManager.GetInstance().ShowPanel<MainPanel>("MainPanel");

        MusicMgr.GetInstance().PlaySound("buttonOn", false);
    }

    public void SetScore(int score)
    {
        textScore.text = score.ToString();
    }
    #endregion
}
