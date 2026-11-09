using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class ProcessMovementMovementHook : IProcessMovementMovementHook
{
    internal event OnProcessMovementMovementPreDelegate? _Pre;
    internal event OnProcessMovementMovementPostDelegate? _Post;

    public event OnProcessMovementMovementPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.ProcessMovement);
            _Pre += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.ProcessMovement);
        }
        remove
        {
            _Pre -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.ProcessMovement);
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.ProcessMovement);
        }
    }

    public event OnProcessMovementMovementPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.ProcessMovement);
            _Post += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.ProcessMovement);
        }
        remove
        {
            _Post -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.ProcessMovement);
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.ProcessMovement);
        }
    }

    public void InvokePre(ref ProcessMovementMovementPreContext ctx) => _Pre?.Invoke(ref ctx);
    public void InvokePost(ref ProcessMovementMovementPostContext ctx) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.ProcessMovement);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.ProcessMovement);
    }
}
