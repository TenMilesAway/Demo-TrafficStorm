using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    private Button btnClose;

    private Slider sliderMusic;
    private Slider sliderSFX;

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

        sliderMusic = GetControl<Slider>("sliderMusic");
        sliderSFX   = GetControl<Slider>("sliderSFX");

        AddListeners();
    }

    private void AddListeners()
    {
        btnClose.onClick.AddListener(OnClose);

        sliderMusic.onValueChanged.AddListener(OnMusicChange);
        sliderSFX.onValueChanged.AddListener(OnSFXChange);
    }

    private void RemoveListeners()
    {
        btnClose.onClick.RemoveListener(OnClose);

        sliderMusic.onValueChanged.RemoveListener(OnMusicChange);
        sliderSFX.onValueChanged.RemoveListener(OnSFXChange);
    }
    #endregion

    #region Main Methods
    private void OnClose()
    {
        UIManager.GetInstance().HidePanel("SettingPanel");
        Time.timeScale = 1f;

        MusicMgr.GetInstance().PlaySound("buttonCancel", false);
    }

    private void OnMusicChange(float value)
    {
        MusicMgr.GetInstance().ChangeBKValue(value / 100);
    }

    private void OnSFXChange(float value)
    {
        MusicMgr.GetInstance().ChangeSoundValue(value / 100);
    }

    public void SetMusicValue(float value)
    {
        sliderMusic.value = value;
    }

    public void SetSFXValue(float value)
    {
        sliderSFX.value = value;
    }
    #endregion
}
