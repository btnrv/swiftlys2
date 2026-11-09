using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class AcceptInputEntityHook : IAcceptInputEntityHook
{
    internal event OnAcceptInputEntityPreDelegate? _Pre;
    internal event OnAcceptInputEntityPostDelegate? _Post;

    public event OnAcceptInputEntityPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.AcceptInput);
            _Pre += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.AcceptInput);
        }
        remove
        {
            _Pre -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.AcceptInput);
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.AcceptInput);
        }
    }

    public event OnAcceptInputEntityPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.AcceptInput);
            _Post += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.AcceptInput);
        }
        remove
        {
            _Post -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.AcceptInput);
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.AcceptInput);
        }
    }

    public void InvokePre( ref AcceptInputEntityPreContext ctx ) => _Pre?.Invoke(ref ctx);
    public void InvokePost( ref AcceptInputEntityPostContext ctx ) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.AcceptInput);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.AcceptInput);
    }
}
