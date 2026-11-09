using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class DuckMovementHook : IDuckMovementHook
{
    internal event OnDuckMovementPreDelegate? _Pre;
    internal event OnDuckMovementPostDelegate? _Post;

    public event OnDuckMovementPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.Duck);
            _Pre += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.Duck);
        }
        remove
        {
            _Pre -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.Duck);
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.Duck);
        }
    }

    public event OnDuckMovementPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.Duck);
            _Post += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.Duck);
        }
        remove
        {
            _Post -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.Duck);
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.Duck);
        }
    }

    public void InvokePre(ref DuckMovementPreContext ctx) => _Pre?.Invoke(ref ctx);
    public void InvokePost(ref DuckMovementPostContext ctx) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.Duck);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.Duck);
    }
}
