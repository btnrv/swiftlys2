using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class PostThinkPawnHook : IPostThinkPawnHook
{
    internal event OnPostThinkPawnPreDelegate? _Pre;
    internal event OnPostThinkPawnPostDelegate? _Post;

    public event OnPostThinkPawnPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.PostThink);
            _Pre += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.PostThink);
        }
        remove
        {
            _Pre -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.PostThink);
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.PostThink);
        }
    }

    public event OnPostThinkPawnPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.PostThink);
            _Post += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.PostThink);
        }
        remove
        {
            _Post -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.PostThink);
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.PostThink);
        }
    }

    public void InvokePre(ref PostThinkPawnPreContext ctx) => _Pre?.Invoke(ref ctx);
    public void InvokePost(ref PostThinkPawnPostContext ctx) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.PostThink);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.PostThink);
    }
}
