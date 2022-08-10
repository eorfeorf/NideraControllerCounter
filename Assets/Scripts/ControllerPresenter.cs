using SharpDX.DirectInput;
using UniRx;
using UnityEngine;

public class ControllerPresenter : MonoBehaviour
{
    [SerializeField]
    private ControllerView controllerView;
    [SerializeField]
    private SettingView settingView;
    [SerializeField]
    private HamburgerMenu.Scripts.HamburgerMenu menu;

    private ControllerModel model;
    private ControllerInput input;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        
        input = gameObject.AddComponent<ControllerInput>();
        model = new ControllerModel(input);
    }

    private void OnDestroy()
    {
        model.Dispose();
    }

    private void Start()
    {
        InitializeViews();

        model.OnKey.Subscribe(key =>
        {
            controllerView.SetButtonState(key.index, key.active);
        }).AddTo(this);
        model.OnScratchL.Subscribe(active =>
        {
            controllerView.SetStateScratchL(active);
        }).AddTo(this);
        model.OnScratchR.Subscribe(active =>
        {
            controllerView.SetStateScratchR(active);
        }).AddTo(this);
        model.OnCounterTotal.Subscribe(total =>
        {
            controllerView.SetTotal(total);
        }).AddTo(this);
    }

    private void Update()
    {
        var perSec = model.UpdatePerSec();
        controllerView.SetKeyPerSec(perSec);
    }

    /// <summary>
    /// View初期化.
    /// </summary>
    private void InitializeViews()
    {
        // メニュー.
        menu.Initialize();
        // 色.
        menu.AddTextField("Color", "#00FFFFFF").Subscribe(value =>
        {
            if (ColorUtility.TryParseHtmlString(value, out var color))
            {
                controllerView.SetColor(color);
                return;
            }
            Debug.LogError("Invalid html color code.");
        }).AddTo(this);
        // プレイサイド.
        menu.AddDropdown<PlaySide>("Side", (int)PlaySide.P1).Subscribe(value =>
        {
            model.SetPlaySide((PlaySide)value);
            controllerView.ApplyPlaySide((PlaySide)value);
        }).AddTo(this);
        menu.HideAll();

        // メニューボタン.
        settingView.OnOpen().Subscribe(_ =>
        {
            settingView.SetActiveCloseButton(true);
            settingView.SetActiveOpenButton(false);
            menu.ShowAll();
        }).AddTo(this);
        settingView.OnClose().Subscribe(_ =>
        {
            settingView.SetActiveCloseButton(false);
            settingView.SetActiveOpenButton(true);
            menu.HideAll();
        }).AddTo(this);

        // カウンター.
        controllerView.SetTotal(0);
        controllerView.SetKeyPerSec(0);

        // ボタン.
        for (var i = 0; i < GameDefine.KEY_NUM; ++i)
        {
            controllerView.SetButtonState(i, false);
        }

        // スクラッチ.
        controllerView.SetStateScratchL(false);
        controllerView.SetStateScratchR(false);
    }
}