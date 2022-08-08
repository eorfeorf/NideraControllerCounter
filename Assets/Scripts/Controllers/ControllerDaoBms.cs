using UnityEngine;

/// <summary>
/// コントローラーの挙動 DAO_BMS
/// </summary>
public class ControllerDaoBms : IController
{
    public ControllerType type => ControllerType.DAO_BMS;
    
    public (bool l, bool r) GetScratchActive(ushort value, PlaySide side)
    {
        var ret = (false, false);
        
        if (value == GameDefine.SCRATCH_DEFAULT_VALUE)
        {
            Debug.Log($"zero : value={value}");

            ret = (false, false);
        }
        if (value < GameDefine.SCRATCH_DEFAULT_VALUE)
        {
            Debug.Log($"minus : value={value}");

            ret = (true, false);
        }
        if (value > GameDefine.SCRATCH_DEFAULT_VALUE)
        {
            Debug.Log($"plus : value={value}");

            ret = (false, true);
        }

        // 2Pなら逆の方向を光らせる.
        if (side == PlaySide.P2)
        {
            ret = (!ret.Item1, !ret.Item2);
        }

        return ret;
    }
}
