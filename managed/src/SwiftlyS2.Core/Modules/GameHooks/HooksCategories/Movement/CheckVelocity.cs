using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class CheckVelocityMovementHook : ICheckVelocityMovementHook
{
    internal event OnCheckVelocityMovementPreDelegate? _Pre;
    internal event OnCheckVelocityMovementPostDelegate? _Post;

    public event OnCheckVelocityMovementPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.CheckVelocity);
            _Pre += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CheckVelocity);
        }
        remove
        {
            _Pre -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CheckVelocity);
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.CheckVelocity);
        }
    }

    public event OnCheckVelocityMovementPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.CheckVelocity);
            _Post += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CheckVelocity);
        }
        remove
        {
            _Post -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CheckVelocity);
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.CheckVelocity);
        }
    }

    public void InvokePre(ref CheckVelocityMovementPreContext ctx) => _Pre?.Invoke(ref ctx);
    public void InvokePost(ref CheckVelocityMovementPostContext ctx) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.CheckVelocity);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.CheckVelocity);
    }
}
