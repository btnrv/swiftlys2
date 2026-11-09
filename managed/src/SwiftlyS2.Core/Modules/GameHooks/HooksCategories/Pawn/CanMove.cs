using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class CanMovePawnHook : ICanMovePawnHook
{
    internal event OnCanMovePawnPreDelegate? _Pre;
    internal event OnCanMovePawnPostDelegate? _Post;

    public event OnCanMovePawnPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.CanMove);
            _Pre += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CanMove);
        }
        remove
        {
            _Pre -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CanMove);
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.CanMove);
        }
    }

    public event OnCanMovePawnPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.CanMove);
            _Post += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CanMove);
        }
        remove
        {
            _Post -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CanMove);
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.CanMove);
        }
    }

    public void InvokePre(ref CanMovePawnPreContext ctx) => _Pre?.Invoke(ref ctx);
    public void InvokePost(ref CanMovePawnPostContext ctx) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.CanMove);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.CanMove);
    }
}
