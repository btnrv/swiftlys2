using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class CanUseWeaponHook : ICanUseWeaponHook
{
    internal event OnCanUseWeaponPreDelegate? _Pre;
    internal event OnCanUseWeaponPostDelegate? _Post;

    public event OnCanUseWeaponPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.CanUse);
            _Pre += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CanUse);
        }
        remove
        {
            _Pre -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CanUse);
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.CanUse);
        }
    }

    public event OnCanUseWeaponPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.CanUse);
            _Post += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CanUse);
        }
        remove
        {
            _Post -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.CanUse);
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.CanUse);
        }
    }

    public void InvokePre(ref CanUseWeaponPreContext ctx) => _Pre?.Invoke(ref ctx);
    public void InvokePost(ref CanUseWeaponPostContext ctx) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.CanUse);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.CanUse);
    }
}
