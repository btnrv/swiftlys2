using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class MoveInitMovementHook : IMoveInitMovementHook
{
    internal event OnMoveInitMovementPreDelegate? _Pre;
    internal event OnMoveInitMovementPostDelegate? _Post;

    public event OnMoveInitMovementPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.MoveInit);
            _Pre += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.MoveInit);
        }
        remove
        {
            _Pre -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.MoveInit);
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.MoveInit);
        }
    }

    public event OnMoveInitMovementPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.MoveInit);
            _Post += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.MoveInit);
        }
        remove
        {
            _Post -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.MoveInit);
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.MoveInit);
        }
    }

    public void InvokePre(ref MoveInitMovementPreContext ctx) => _Pre?.Invoke(ref ctx);
    public void InvokePost(ref MoveInitMovementPostContext ctx) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.MoveInit);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.MoveInit);
    }
}
