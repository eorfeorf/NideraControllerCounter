using UnityEngine;

/// <summary>
/// コントローラーの挙動 DAO_INF
/// </summary>
public class ControllerDaoInf : IController
{
    public ControllerType type => ControllerType.DAO_INF;

    private ushort prev = GameDefine.SCRATCH_DEFAULT_VALUE;
    
    public (bool l, bool r) GetScratchActive(ushort value, PlaySide side)
    {
        var ret = (false, false);

        ushort tmpValue = value;
        ushort tmpPrev = prev;
        
        // 中間よりどっちに寄っているか？.
        if (prev > GameDefine.SCRATCH_DEFAULT_VALUE && value > GameDefine.SCRATCH_DEFAULT_VALUE)
        {
            // 大きい場合は引く.
            tmpValue = (ushort) (value - GameDefine.SCRATCH_DEFAULT_VALUE);
            tmpPrev = (ushort) (prev - GameDefine.SCRATCH_DEFAULT_VALUE);
        }
        else if (prev < GameDefine.SCRATCH_DEFAULT_VALUE && prev < GameDefine.SCRATCH_DEFAULT_VALUE)
        {
            // 小さい場合は足す.
            tmpValue = (ushort) (value + GameDefine.SCRATCH_DEFAULT_VALUE);
            tmpPrev = (ushort) (prev + GameDefine.SCRATCH_DEFAULT_VALUE);
        }
        
        var sub = tmpValue - tmpPrev;
        
        if (sub > 0)
        {
            ret = (true, false);
        }
        else if(sub < 0)
        {
            ret = (false, true);
        }
        else
        {
            ret = (false, false);
        }

        prev = value;
        
        // 2Pなら逆の方向を光らせる.
        if (side == PlaySide.P2)
        {
            (ret.Item1, ret.Item2) = (ret.Item2, ret.Item1);
        }
        
        return ret;
    }
}
