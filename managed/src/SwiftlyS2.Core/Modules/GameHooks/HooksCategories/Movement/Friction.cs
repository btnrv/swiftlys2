using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class FrictionMovementHook : IFrictionMovementHook
{
    internal event OnFrictionMovementPreDelegate? _Pre;
    internal event OnFrictionMovementPostDelegate? _Post;

    public event OnFrictionMovementPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.Friction);
            _Pre += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.Friction);
        }
        remove
        {
            _Pre -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.Friction);
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.Friction);
        }
    }

    public event OnFrictionMovementPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.Friction);
            _Post += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.Friction);
        }
        remove
        {
            _Post -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.Friction);
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.Friction);
        }
    }

    public void InvokePre(ref FrictionMovementPreContext ctx) => _Pre?.Invoke(ref ctx);
    public void InvokePost(ref FrictionMovementPostContext ctx) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.Friction);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.Friction);
    }
}
