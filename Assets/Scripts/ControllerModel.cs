using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class ControllerModel : IDisposable
{
    public IReadOnlyReactiveProperty<KeyActivator.Key> OnKey => onKey;
    private ReactiveProperty<KeyActivator.Key> onKey = new ReactiveProperty<KeyActivator.Key>();
    public IReadOnlyReactiveProperty<bool> OnScratchL => onScratchL;
    private ReactiveProperty<bool> onScratchL = new ReactiveProperty<bool>();
    public IReadOnlyReactiveProperty<bool> OnScratchR => onScratchR;
    private ReactiveProperty<bool> onScratchR = new ReactiveProperty<bool>();
    public IReadOnlyReactiveProperty<int> OnCounterTotal => onCounterTotal;
    private ReactiveProperty<int> onCounterTotal = new ReactiveProperty<int>();

    private CompositeDisposable disposable = new CompositeDisposable();

    private ControllerInput input;
    private readonly Counter counter = new Counter();
    private readonly KeyActivator keyActivator = new KeyActivator();
    private readonly ScratchActivator scratchActivator = new ScratchActivator();

    private IController currentController;
    private readonly ControllerRepository controllerRepository = new ControllerRepository();
    private readonly ReactiveProperty<ControllerType>  controllerType = new ReactiveProperty<ControllerType>();
    private ReactiveProperty<PlaySide> playSide = new ReactiveProperty<PlaySide>();

    public int PerSec => counter.GetPerSec();

    public ControllerModel(ControllerInput input)
    {
        this.input = input;
        controllerType.Value = ControllerType.DAO_INF;
        
        controllerType.Subscribe(type =>
        {
            currentController = controllerRepository.GetController(type);
        }).AddTo(disposable);
        
        playSide.Subscribe(side =>
        {
            
        }).AddTo(disposable);
        
        // 入力の状態更新.
        input.JoystickState.SkipLatestValueOnSubscribe().Subscribe(state =>
        {
            // 鍵盤.
            keyActivator.Update(state);
            
            // スクラッチ.
            var scratchActive = currentController.GetScratchActive((ushort)state.X, playSide.Value);
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
        
        // KeyParSec.
        counter.PerSecRun().Forget();
    }

    public void Update()
    {
        counter.UpdatePerSec();
    }

    public void Dispose()
    {
        disposable?.Dispose();
    }
}
