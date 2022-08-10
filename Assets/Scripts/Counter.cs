using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

/// <summary>
/// カウンター.
/// </summary>
public class Counter
{
    public IReadOnlyReactiveProperty<int> Total => total;
    private readonly ReactiveProperty<int> total = new ReactiveProperty<int>();

    private readonly int[] count = new int[GameDefine.KEY_NUM];
    private readonly List<float> perSec = new List<float>();
    
    
    public int GetCount(int index)
    {
        return count[index];
    }

    public int GetPerSec()
    {
        return perSec.Count;
    }

    public void Add(int index, int addNum = 1)
    {
        perSec.Add(Time.time);
        count[index] += addNum;
        total.Value = count.Sum();
    }
    
    public int UpdatePerSecRun()
    {
        for (var i = 0; i < perSec.Count; ++i)
        {
            if ((perSec[i] + 1f) < Time.time)
            {
                perSec.RemoveAt(i);
            }
        }

        return perSec.Count;
    }
}