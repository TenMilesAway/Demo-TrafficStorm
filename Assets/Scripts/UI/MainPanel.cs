using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WeChatWASM;

public class MainPanel : BasePanel
{
    [SerializeField] private Image[] HealthImgs;
    [SerializeField] private float InitialHealth;

    private float currentHealth;
    private float lastHealth;
    private int currentScore;

    private TMP_Text textScore;
    private TMP_Text textRemoveCollisionNum;
    private TMP_Text textRoadBlockNum;
    private TMP_Text textSpeedUpNum;

    private Button btnBack;
    private Button btnStop;
    private Button btnSetting;
    private Button btnRemoveCollision;
    private Button btnRoadBlock;
    private Button btnSpeedUp;

    private Image imgBtnRemoveCollision;
    private Image imgBtnRoadBlock;
    private Image imgBtnSpeedUp;
    private List<Image> btnImgs;

    private Button selectedButton;

    private Color btnColor;

    #region Unity 生命周期
    protected override void Start()
    {
        currentScore = 0;
        currentHealth = InitialHealth;

        // 初始化
        ItemManager.GetInstance().InitData();
        GeneratePool.GetInstance().InitData();

        textScore.text = currentScore.ToString();
        textRemoveCollisionNum.text = ItemManager.GetInstance().GetItemAmount(ItemType.RemoveCollision).ToString();
        textRoadBlockNum.text       = ItemManager.GetInstance().GetItemAmount(ItemType.RoadBlock).ToString();
        textSpeedUpNum.text         = ItemManager.GetInstance().GetItemAmount(ItemType.SpeedUp).ToString();

        AddListeners();
        EventCenter.GetInstance().EventTrigger("UpdateUI");

        MusicMgr.GetInstance().PlayBkMusic("one");
    }

    protected override void OnDestroy()
    {
        RemoveListeners();
    }
    #endregion

    #region Init Methods
    protected override void InitUI()
    {
        textScore              = GetControl<TMP_Text>("textScore");
        textRemoveCollisionNum = GetControl<TMP_Text>("textRemoveCollisionNum");
        textRoadBlockNum       = GetControl<TMP_Text>("textRoadBlockNum");
        textSpeedUpNum         = GetControl<TMP_Text>("textSpeedUpNum");

        btnRemoveCollision = GetControl<Button>("btnRemoveCollision");
        btnRoadBlock       = GetControl<Button>("btnRoadBlock");
        btnSpeedUp         = GetControl<Button>("btnSpeedUp");
        btnBack            = GetControl<Button>("btnBack");
        btnStop            = GetControl<Button>("btnStop");
        btnSetting         = GetControl<Button>("btnSetting");

        imgBtnRemoveCollision = btnRemoveCollision.GetComponent<Image>();
        imgBtnRoadBlock       = btnRoadBlock.GetComponent<Image>();
        imgBtnSpeedUp         = btnSpeedUp.GetComponent<Image>();

        btnColor = imgBtnRemoveCollision.color;

        btnImgs = new List<Image>();
        btnImgs.Add(imgBtnRemoveCollision);
        btnImgs.Add(imgBtnRoadBlock);
        btnImgs.Add(imgBtnSpeedUp);
    }

    private void AddListeners()
    {
        btnRemoveCollision.onClick.AddListener(OnRemoveCollision);
        btnRoadBlock.onClick.AddListener(OnRoadBlock);
        btnSpeedUp.onClick.AddListener(OnSpeedUp);
        btnSetting.onClick.AddListener(OnSetting);
        btnStop.onClick.AddListener(OnStop);
        btnBack.onClick.AddListener(OnBack);

        EventCenter.GetInstance().AddEventListener("UpdateUI", UpdateUI);
        EventCenter.GetInstance().AddEventListener<BonusType>("AddScore", AddScore);
        EventCenter.GetInstance().AddEventListener("ResetSelectedItem", ResetSelectedItem);
        EventCenter.GetInstance().AddEventListener<float>("UpdateHealth", UpdateHealth);
    }

    private void RemoveListeners()
    {
        btnRemoveCollision.onClick.RemoveListener(OnRemoveCollision);
        btnRoadBlock.onClick.RemoveListener(OnRoadBlock);
        btnSpeedUp.onClick.RemoveListener(OnSpeedUp);
        btnSetting.onClick.RemoveListener(OnSetting);
        btnStop.onClick.RemoveListener(OnStop);
        btnBack.onClick.RemoveListener(OnBack);

        EventCenter.GetInstance().RemoveEventListener("UpdateUI", UpdateUI);
        EventCenter.GetInstance().RemoveEventListener<BonusType>("AddScore", AddScore);
        EventCenter.GetInstance().RemoveEventListener("ResetSelectedItem", ResetSelectedItem);
        EventCenter.GetInstance().RemoveEventListener<float>("UpdateHealth", UpdateHealth);
    }
    #endregion

    #region Main Methods
    private void OnRemoveCollision()
    {
        OnItemButtonClick(btnRemoveCollision, ItemType.RemoveCollision);
    }

    private void OnRoadBlock()
    {
        OnItemButtonClick(btnRoadBlock, ItemType.RoadBlock);
    }

    private void OnSpeedUp()
    {
        OnItemButtonClick(btnSpeedUp, ItemType.SpeedUp);
    }

    private void OnSetting()
    {
        UIManager.GetInstance().ShowPanel<SettingPanel>("SettingPanel", E_UI_Layer.Top, (panel) =>
        {
            panel.SetMusicValue(MusicMgr.GetInstance().GetBKValue() * 100);
            panel.SetSFXValue(MusicMgr.GetInstance().GetSFXValue() * 100);
        });
        Time.timeScale = 0f;
    }

    private void OnStop()
    {
        UIManager.GetInstance().ShowPanel<StopPanel>("StopPanel");
        Time.timeScale = 0f;
    }

    private void OnBack()
    {
        DestoryAll();

        UIManager.GetInstance().HidePanel("MainPanel");
        UIManager.GetInstance().ShowPanel<StartPanel>("StartPanel");
    }

    private void OnItemButtonClick(Button btn, ItemType type)
    {
        // 如果再次点击选中的按钮，视为取消
        if (selectedButton == btn)
        {
            HighlightItem(btn, type);

            MusicMgr.GetInstance().PlaySound("buttonCancel", false);

            return;
        }

        if (ItemManager.GetInstance().GetItemAmount(type) == 0)
        {
            Debug.LogWarning($"道具 {type} 已耗尽");
            MusicMgr.GetInstance().PlaySound("fail", false);
        }
        else
        {
            HighlightItem(btn, type);

            selectedButton = btn;

            MusicMgr.GetInstance().PlaySound("buttonOn", false);

            if (type == ItemType.RemoveCollision)
            {
                ItemManager.GetInstance().RemoveCollisionState = true;
            }
            else if (type == ItemType.SpeedUp)
            {
                ItemManager.GetInstance().SpeedUpState = true;
            }
            // 这个道具点击即用，所以要 Reset
            else if (type == ItemType.RoadBlock)
            {
                ItemManager.GetInstance().UseItem(ItemType.RoadBlock, null);
                ResetSelectedItem();
            }
        }
    }

    private void HighlightItem(Button btn, ItemType type)
    {
        Image btnImg = null;

        switch (type)
        {
            case ItemType.RemoveCollision:
                btnImg = imgBtnRemoveCollision;
                break;
            case ItemType.RoadBlock:
                btnImg = imgBtnRoadBlock;
                break;
            case ItemType.SpeedUp:
                btnImg = imgBtnSpeedUp;
                break;
        }

        if (selectedButton == btn)
        {
            ResetSelectedItem();
        }
        else
        {
            ResetSelectedItem();
            btnImg.color = Color.white;
        }
    }

    private void ResetSelectedItem()
    {
        foreach (Image img in btnImgs) img.color = btnColor;

        selectedButton = null;

        ItemManager.GetInstance().ResetAllStates();
    }

    private void UpdateUI()
    {
        // 积分更新
        textScore.text = currentScore.ToString();

        // 道具卡更新
        textRemoveCollisionNum.text = ItemManager.GetInstance().GetItemAmount(ItemType.RemoveCollision).ToString();
        textRoadBlockNum.text       = ItemManager.GetInstance().GetItemAmount(ItemType.RoadBlock).ToString();
        textSpeedUpNum.text         = ItemManager.GetInstance().GetItemAmount(ItemType.SpeedUp).ToString();

        // 血量更新
        if (currentHealth != lastHealth)
        {
            for (int i = 0; i < HealthImgs.Length; i++)
            {
                if (i < currentHealth)
                    HealthImgs[i].gameObject.SetActive(true);
                else
                    HealthImgs[i].gameObject.SetActive(false);
            }
            lastHealth = currentHealth;
        }
        
    }

    private void UpdateHealth(float amount)
    {
        // 血条更新
        currentHealth -= amount;
        UpdateUI();

        if (currentHealth == 0f)
        {
            print(currentHealth);
            GameOver();
        }
    }

    private void AddScore(BonusType type)
    {
        float proOfGettingItems = 0;

        switch (type)
        {
            case BonusType.Normal:
                currentScore += 10;
                proOfGettingItems = 0.01f;
                break;
            case BonusType.Excellent:
                currentScore += 15;
                proOfGettingItems = 0.05f;
                break;
            case BonusType.Epic:
                currentScore += 20;
                proOfGettingItems = 0.10f;
                break;
            case BonusType.Legend:
                currentScore += 50;
                proOfGettingItems = 0.30f;
                break;
            case BonusType.Myth:
                currentScore += 100;
                proOfGettingItems = 0.50f;
                break;
        }

        ItemManager.GetInstance().AddItemByPro(1, proOfGettingItems);
    }

    private void GameOver()
    {
        DestoryAll();
        // 记录当前分数
        SendScore(currentScore);
        // 展示结束 UI
        UIManager.GetInstance().ShowPanel<GameOverPanel>("GameOverPanel", E_UI_Layer.Top, (panel) => 
        {
            panel.SetScore(currentScore);
        });
    }

    public void DestoryAll()
    {
        // 停止对象池生成
        GeneratePool.GetInstance().StopGenerate();
        // 获得场景所有 BaseMovement，销毁
        BaseMovement[] allEntities = Transform.FindObjectsOfType<BaseMovement>();
        foreach (BaseMovement entity in allEntities)
        {
            Destroy(entity.gameObject);
        }
    }

    private void SendScore(int score)
    {
        OpenDataMessage msgData = new OpenDataMessage();

        msgData.type = "setUserRecord";
        msgData.score = score;
        msgData.rankType = "gameScore";

        string msg = JsonUtility.ToJson(msgData);
        WX.GetOpenDataContext().PostMessage(msg);
    }
    #endregion
}
