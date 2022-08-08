public static class GameDefine
{
    public const int KEY_NUM = 7;
    public const ushort SCRATCH_MIN_VALUE = 0;
    public const ushort SCRATCH_MAX_VALUE = ushort.MaxValue;
    public const ushort SCRATCH_DEFAULT_VALUE = SCRATCH_MAX_VALUE / 2;
    public const float SCRATCH_POLLING_INTERVAL = 0.05f;
}
    
public enum PlaySide
{
    P1,P2
}

public enum ControllerType
{
    DAO_BMS,
    DAO_INF,
}