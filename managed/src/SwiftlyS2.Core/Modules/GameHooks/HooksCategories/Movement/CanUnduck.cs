using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class CanUnduckMovementHook : ICanUnduckMovementHook
{
    internal event OnCanUnduckMovementPreDelegate? _Pre;
    internal event OnCanUnduckMovementPostDelegate? _Post;

    public event OnCanUnduckMovementPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.CanUnduck);
            _Pre += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CanUnduck);
        }
        remove
        {
            _Pre -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CanUnduck);
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.CanUnduck);
        }
    }

    public event OnCanUnduckMovementPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.CanUnduck);
            _Post += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CanUnduck);
        }
        remove
        {
            _Post -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CanUnduck);
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.CanUnduck);
        }
    }

    public void InvokePre(ref CanUnduckMovementPreContext ctx) => _Pre?.Invoke(ref ctx);
    public void InvokePost(ref CanUnduckMovementPostContext ctx) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.CanUnduck);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.CanUnduck);
    }
}
