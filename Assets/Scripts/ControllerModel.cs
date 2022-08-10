using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class ControllerModel : IDisposable
{
    // 鍵盤.
    public IReadOnlyReactiveProperty<KeyActivator.Key> OnKey => onKey;
    private readonly ReactiveProperty<KeyActivator.Key> onKey = new ReactiveProperty<KeyActivator.Key>();
    // スクラッチ.
    public IReadOnlyReactiveProperty<bool> OnScratchL => onScratchL;
    private readonly ReactiveProperty<bool> onScratchL = new ReactiveProperty<bool>();
    public IReadOnlyReactiveProperty<bool> OnScratchR => onScratchR;
    private readonly ReactiveProperty<bool> onScratchR = new ReactiveProperty<bool>();
    // 合計.
    public IReadOnlyReactiveProperty<int> OnCounterTotal => onCounterTotal;
    private readonly ReactiveProperty<int> onCounterTotal = new ReactiveProperty<int>();

    // 機能.
    private readonly Counter counter = new Counter();
    private readonly KeyActivator keyActivator = new KeyActivator();
    private readonly ScratchActivator scratchActivator = new ScratchActivator();

    // コントローラー.
    private IController currentController;
    private readonly ControllerRepository controllerRepository = new ControllerRepository();
    private readonly ReactiveProperty<ControllerType>  controllerType = new ReactiveProperty<ControllerType>();
    private PlaySide playSide = PlaySide.P1;
    
    private readonly CompositeDisposable disposable = new CompositeDisposable();

    public ControllerModel(ControllerInput input)
    {
        controllerType.Value = ControllerType.DAO_INF;
        
        // コントローラー.
        controllerType.Subscribe(type =>
        {
            currentController = controllerRepository.GetController(type);
        }).AddTo(disposable);
        
        // 入力の状態更新.
        input.JoystickState.SkipLatestValueOnSubscribe().Subscribe(state =>
        {
            // 鍵盤.
            keyActivator.Update(state);
            
            // スクラッチ.
            var scratchActive = currentController.GetScratchActive((ushort)state.X, playSide);
            Debug.Log($"L:R -> ({scratchActive.l}, {scratchActive.r})");
            scratchActivator.Update(scratchActive.l, scratchActive.r);
        }).AddTo(disposable);

        // ボタンが押された時の処理.
        for (var i = 0; i < GameDefine.KEY_NUM; ++i)
        {
            keyActivator.Keys[i].Subscribe(key =>
            {
                if (key.active)
                {
                    counter.Add(key.index);
                }
                onKey.Value = key;
            }).AddTo(disposable);
        }
        
        // スクラッチが回された時の処理.
        scratchActivator.ScratchL.SkipLatestValueOnSubscribe().Subscribe(active =>
        {
            onScratchL.Value = active;
        }).AddTo(disposable);
        scratchActivator.ScratchR.SkipLatestValueOnSubscribe().Subscribe(active =>
        {
            onScratchR.Value = active;
        }).AddTo(disposable);
        
        // カウンターが更新された時の処理.
        counter.Total.Subscribe(total =>
        {
            onCounterTotal.Value = total;
        }).AddTo(disposable);
        }

    public int UpdatePerSec() => counter.UpdatePerSecRun();

    public void Dispose()
    {
        disposable?.Dispose();
    }

    public void SetPlaySide(PlaySide side)
    {
        playSide = side;
    }
}
