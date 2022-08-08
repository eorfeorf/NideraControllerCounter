using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ScratchActivator
{
    public IReactiveProperty<bool> ScratchL => scratchL;
    private readonly ReactiveProperty<bool>scratchL = new ReactiveProperty<bool>();
    public IReactiveProperty<bool> ScratchR => scratchR;
    private readonly ReactiveProperty<bool> scratchR = new ReactiveProperty<bool>();
    
    public void Update(bool activeL, bool activeR)
    {
        // L.
        scratchL.Value = activeL;
        // R.
        scratchR.Value = activeR;
    }
}
