using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class OnJumpLegacyMovementHook : IOnJumpLegacyMovementHook
{
    internal event OnOnJumpLegacyMovementPreDelegate? _Pre;
    internal event OnOnJumpLegacyMovementPostDelegate? _Post;

    public event OnOnJumpLegacyMovementPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.OnJumpLegacy);
            _Pre += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.OnJumpLegacy);
        }
        remove
        {
            _Pre -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.OnJumpLegacy);
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.OnJumpLegacy);
        }
    }

    public event OnOnJumpLegacyMovementPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.OnJumpLegacy);
            _Post += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.OnJumpLegacy);
        }
        remove
        {
            _Post -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.OnJumpLegacy);
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.OnJumpLegacy);
        }
    }

    public void InvokePre(ref OnJumpLegacyMovementPreContext ctx) => _Pre?.Invoke(ref ctx);
    public void InvokePost(ref OnJumpLegacyMovementPostContext ctx) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.OnJumpLegacy);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.OnJumpLegacy);
    }
}
