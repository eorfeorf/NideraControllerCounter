/// <summary>
/// コントローラー.
/// </summary>
public interface IController
{
   /// <summary>
   /// スクラッチの値をActiveに変換.
   /// </summary>
   /// <param name="value"></param>
   /// <param name="side"></param>
   /// <returns></returns>
   public (bool l, bool r) GetScratchActive(ushort value, PlaySide side);
   public ControllerType type { get; }
}
