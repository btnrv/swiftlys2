using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class CategorizePositionMovementHook : ICategorizePositionMovementHook
{
    internal event OnCategorizePositionMovementPreDelegate? _Pre;
    internal event OnCategorizePositionMovementPostDelegate? _Post;

    public event OnCategorizePositionMovementPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.CategorizePosition);
            _Pre += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CategorizePosition);
        }
        remove
        {
            _Pre -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CategorizePosition);
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.CategorizePosition);
        }
    }

    public event OnCategorizePositionMovementPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.CategorizePosition);
            _Post += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CategorizePosition);
        }
        remove
        {
            _Post -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CategorizePosition);
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.CategorizePosition);
        }
    }

    public void InvokePre(ref CategorizePositionMovementPreContext ctx) => _Pre?.Invoke(ref ctx);
    public void InvokePost(ref CategorizePositionMovementPostContext ctx) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.CategorizePosition);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.CategorizePosition);
    }
}
