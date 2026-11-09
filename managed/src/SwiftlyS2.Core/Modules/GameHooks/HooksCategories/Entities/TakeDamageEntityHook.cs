using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class TakeDamageEntityHook : ITakeDamageEntityHook
{
    internal event OnTakeDamageEntityPreDelegate? _Pre;
    internal event OnTakeDamageEntityPostDelegate? _Post;

    public event OnTakeDamageEntityPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.TakeDamage);
            _Pre += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.TakeDamage);
        }
        remove
        {
            _Pre -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.TakeDamage);
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.TakeDamage);
        }
    }

    public event OnTakeDamageEntityPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.TakeDamage);
            _Post += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.TakeDamage);
        }
        remove
        {
            _Post -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.TakeDamage);
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.TakeDamage);
        }
    }

    public void InvokePre( ref TakeDamageEntityPreContext ctx ) => _Pre?.Invoke(ref ctx);
    public void InvokePost( ref TakeDamageEntityPostContext ctx ) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.TakeDamage);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.TakeDamage);
    }
}
