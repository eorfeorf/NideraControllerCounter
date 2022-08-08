using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using SharpDX.DirectInput;


public class ControllerInput : MonoBehaviour
{
    public IReadOnlyReactiveProperty<JoystickState> JoystickState => joystickState;
    private readonly ReactiveProperty<JoystickState> joystickState = new ReactiveProperty<JoystickState>();

    private Joystick joystick;

    private void Awake()
    {
        // デバイスの初期化.
        var dinput = new DirectInput();
        var joystickGuid = Guid.Empty;
        if (joystickGuid == Guid.Empty)
        {
            foreach (var device in dinput.GetDevices(SharpDX.DirectInput.DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
            {
                // 弐寺コンだといいなぁ～.
                joystickGuid = device.InstanceGuid;
                break;
            }
        }

        joystick = new Joystick(dinput, joystickGuid);
        joystick.Properties.BufferSize = 1024;

        // ポーリングが早いと変動量が変わらない時があり、点滅するので秒数を指定して間隔を空ける.
        InvokeRepeating(nameof(Polling), 0f, GameDefine.SCRATCH_POLLING_INTERVAL);
    }

    private void Polling()
    {
        joystick.Acquire();
        joystick.Poll();

        var state = joystick.GetCurrentState();
        if (state == null)
        {
            return;
        }

        joystickState.SetValueAndForceNotify(state);
    }
}