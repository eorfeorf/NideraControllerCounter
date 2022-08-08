using UniRx;
using UnityEngine;

public class ControllerPresenter : MonoBehaviour
{
    [SerializeField]
    private ControllerView controllerView;
    [SerializeField]
    private SettingView settingView;
    [SerializeField]
    private HamburgerMenu.Scripts.HamburgerMenu hamburgerMenu;

    private ControllerModel model;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        
        var input = gameObject.AddComponent<ControllerInput>();
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
        controllerView.SetKeyPerSec(model.PerSec);
    }

    /// <summary>
    /// View初期化.
    /// </summary>
    private void InitializeViews()
    {
        // メニュー.
        hamburgerMenu.Initialize();
        hamburgerMenu.AddTextField("Color", "00FFFFFF").Subscribe(x =>
        {
            if (ColorUtility.TryParseHtmlString(x, out var color))
            {
                controllerView.SetColor(color);
            }
        }).AddTo(this);
        hamburgerMenu.HideAll();

        // メニューボタン.
        settingView.OnOpen().Subscribe(_ =>
        {
            settingView.SetActiveCloseButton(true);
            settingView.SetActiveOpenButton(false);
            hamburgerMenu.ShowAll();
        }).AddTo(this);
        settingView.OnClose().Subscribe(_ =>
        {
            settingView.SetActiveCloseButton(false);
            settingView.SetActiveOpenButton(true);
            hamburgerMenu.HideAll();
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