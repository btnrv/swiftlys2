using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class OnJumpModernMovementHook : IOnJumpModernMovementHook
{
    internal event OnOnJumpModernMovementPreDelegate? _Pre;
    internal event OnOnJumpModernMovementPostDelegate? _Post;

    public event OnOnJumpModernMovementPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.OnJumpModern);
            _Pre += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.OnJumpModern);
        }
        remove
        {
            _Pre -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.OnJumpModern);
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.OnJumpModern);
        }
    }

    public event OnOnJumpModernMovementPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.OnJumpModern);
            _Post += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.OnJumpModern);
        }
        remove
        {
            _Post -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.OnJumpModern);
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.OnJumpModern);
        }
    }

    public void InvokePre(ref OnJumpModernMovementPreContext ctx) => _Pre?.Invoke(ref ctx);
    public void InvokePost(ref OnJumpModernMovementPostContext ctx) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.OnJumpModern);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.OnJumpModern);
    }
}
