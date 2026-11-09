using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class FullWalkMoveMovementHook : IFullWalkMoveMovementHook
{
    internal event OnFullWalkMoveMovementPreDelegate? _Pre;
    internal event OnFullWalkMoveMovementPostDelegate? _Post;

    public event OnFullWalkMoveMovementPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.FullWalkMove);
            _Pre += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.FullWalkMove);
        }
        remove
        {
            _Pre -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.FullWalkMove);
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.FullWalkMove);
        }
    }

    public event OnFullWalkMoveMovementPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.FullWalkMove);
            _Post += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.FullWalkMove);
        }
        remove
        {
            _Post -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.FullWalkMove);
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.FullWalkMove);
        }
    }

    public void InvokePre(ref FullWalkMoveMovementPreContext ctx) => _Pre?.Invoke(ref ctx);
    public void InvokePost(ref FullWalkMoveMovementPostContext ctx) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.FullWalkMove);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.FullWalkMove);
    }
}
