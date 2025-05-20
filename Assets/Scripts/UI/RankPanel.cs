using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WeChatWASM;

/// <summary>
/// 开放域
/// </summary>
public class OpenDataMessage
{
    public string type;       // 事件类型
    public string rankType;   // 排行榜类型
    public int score;         // 分数
}

public class RankPanel : BasePanel
{
    private Button btnClose;

    private RawImage rawimgRank;

    #region Unity 生命周期
    protected override void Start()
    {
        InitUI();
        InitRank();
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
        rawimgRank = GetControl<RawImage>("rawimgRank");

        AddListeners();
    }

    private void InitRank()
    {
        RawImage rankBody = rawimgRank;
        RectTransform rectTransform = rankBody.GetComponent<RectTransform>();
        CanvasScaler scaler = GetComponentInParent<Canvas>().GetComponent<CanvasScaler>();

        Vector2 referenceResolution = scaler.referenceResolution;
        float width = rankBody.rectTransform.rect.width * (Screen.height / referenceResolution.y);
        float height = rankBody.rectTransform.rect.height * (Screen.height / referenceResolution.y);
        float posX = rectTransform.anchoredPosition.x;
        float posY = rectTransform.anchoredPosition.y;

        posX = Screen.width / 2 - (width / 2 - Mathf.Abs(posX));
        posY = Screen.height / 2 - (height / 2 - Mathf.Abs(posY));

        WX.ShowOpenData(rankBody.texture, (int)posX, (int)posY, (int)width, (int)height);
        OpenDataMessage msgData = new OpenDataMessage();
        msgData.type = "showFriendsRank";
        msgData.rankType = "gameScore";
        string msg = JsonUtility.ToJson(msgData);
        WX.GetOpenDataContext().PostMessage(msg);
    }

    private void AddListeners()
    {
        btnClose.onClick.AddListener(OnClose);
    }

    private void RemoveListeners()
    {
        btnClose.onClick.RemoveListener(OnClose);
    }
    #endregion

    #region Main Methods
    private void OnClose()
    {
        WX.HideOpenData();
        UIManager.GetInstance().HidePanel("RankPanel");
        MusicMgr.GetInstance().PlaySound("buttonCancel", false);
    }
    #endregion
}
