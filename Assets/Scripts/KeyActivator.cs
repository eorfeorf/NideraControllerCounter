using SharpDX.DirectInput;
using UniRx;

public class KeyActivator
{
    public struct Key
    {
        public int index;
        public bool active;
    }

    public IReadOnlyReactiveProperty<Key>[] Keys => keys;
    private readonly ReactiveProperty<Key>[] keys = new ReactiveProperty<Key>[GameDefine.KEY_NUM];

    public KeyActivator()
    {
        for (var i = 0; i < GameDefine.KEY_NUM; ++i)
        {
            keys[i] = new ReactiveProperty<Key>();
        }
    }

    public void Update(JoystickState state)
    {
        for (var i = 0; i < state.Buttons.Length; ++i)
        {
            if (0 <= i && i < keys.Length)
            {
                keys[i].Value = new Key()
                {
                    index = i,
                    active = state.Buttons[i]
                };
            }
        }
    }
}