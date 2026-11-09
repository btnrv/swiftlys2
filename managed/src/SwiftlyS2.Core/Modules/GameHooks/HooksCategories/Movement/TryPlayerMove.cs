using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class TryPlayerMoveMovementHook : ITryPlayerMoveMovementHook
{
    internal event OnTryPlayerMoveMovementPreDelegate? _Pre;
    internal event OnTryPlayerMoveMovementPostDelegate? _Post;

    public event OnTryPlayerMoveMovementPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.TryPlayerMove);
            _Pre += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.TryPlayerMove);
        }
        remove
        {
            _Pre -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.TryPlayerMove);
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.TryPlayerMove);
        }
    }

    public event OnTryPlayerMoveMovementPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.TryPlayerMove);
            _Post += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.TryPlayerMove);
        }
        remove
        {
            _Post -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.TryPlayerMove);
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.TryPlayerMove);
        }
    }

    public void InvokePre(ref TryPlayerMoveMovementPreContext ctx) => _Pre?.Invoke(ref ctx);
    public void InvokePost(ref TryPlayerMoveMovementPostContext ctx) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.TryPlayerMove);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.TryPlayerMove);
    }
}
