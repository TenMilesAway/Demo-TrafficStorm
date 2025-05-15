using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainPanel : BasePanel
{
    [SerializeField] private Image[] HealthImgs;
    [SerializeField] private int InitialHealth;

    private int currentHealth;
    private int currentScore;

    private TMP_Text textScore;
    private TMP_Text textRemoveCollisionNum;
    private TMP_Text textRoadBlockNum;
    private TMP_Text textSpeedUpNum;

    private Button btnStop;
    private Button btnSetting;
    private Button btnRemoveCollision;
    private Button btnRoadBlock;
    private Button btnSpeedUp;

    private Button selectedButton;

    #region Unity 生命周期
    protected override void Start()
    {
        currentScore = 0;

        textScore.text = currentScore.ToString();
        textRemoveCollisionNum.text = ItemManager.GetInstance().GetItemAmount(ItemType.RemoveCollision).ToString();
        textRoadBlockNum.text       = ItemManager.GetInstance().GetItemAmount(ItemType.RoadBlock).ToString();
        textSpeedUpNum.text         = ItemManager.GetInstance().GetItemAmount(ItemType.SpeedUp).ToString();

        AddListeners();
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
        btnStop            = GetControl<Button>("btnStop");
        btnSetting         = GetControl<Button>("btnSetting");
    }

    private void AddListeners()
    {
        btnRemoveCollision.onClick.AddListener(OnRemoveCollision);
        btnRoadBlock.onClick.AddListener(OnRoadBlock);
        btnSpeedUp.onClick.AddListener(OnSpeedUp);
    }

    private void RemoveListeners()
    {
        btnRemoveCollision.onClick.RemoveListener(OnRemoveCollision);
        btnRoadBlock.onClick.RemoveListener(OnRoadBlock);
        btnSpeedUp.onClick.RemoveListener(OnSpeedUp);
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

    private void OnItemButtonClick(Button btn, ItemType type)
    {
        // 音效

        // 如果再次点击选中的按钮，视为取消
        if (selectedButton == btn)
        {
            return;
        }

        if (ItemManager.GetInstance().GetItemAmount(type) == 0)
        {
            Debug.LogWarning($"道具 {type} 已耗尽");
        }
        else
        {
            selectedButton = btn;

            if (type == ItemType.RemoveCollision)
            {
                ItemManager.GetInstance().RemoveCollisionState = true;
            }
            else if (type == ItemType.SpeedUp)
            {
                ItemManager.GetInstance().SpeedUpState = true;
            }
            else if (type == ItemType.RoadBlock)
            {
                ItemManager.GetInstance().UseItem(ItemType.RoadBlock, null);
                selectedButton = null;
            }
        }
    }

    public void UpdateUI()
    {

    }
    #endregion
}
