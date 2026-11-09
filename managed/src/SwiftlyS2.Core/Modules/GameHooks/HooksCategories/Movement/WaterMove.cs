using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class WaterMoveMovementHook : IWaterMoveMovementHook
{
    internal event OnWaterMoveMovementPreDelegate? _Pre;
    internal event OnWaterMoveMovementPostDelegate? _Post;

    public event OnWaterMoveMovementPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.WaterMove);
            _Pre += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.WaterMove);
        }
        remove
        {
            _Pre -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.WaterMove);
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.WaterMove);
        }
    }

    public event OnWaterMoveMovementPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.WaterMove);
            _Post += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.WaterMove);
        }
        remove
        {
            _Post -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.WaterMove);
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.WaterMove);
        }
    }

    public void InvokePre(ref WaterMoveMovementPreContext ctx) => _Pre?.Invoke(ref ctx);
    public void InvokePost(ref WaterMoveMovementPostContext ctx) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.WaterMove);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.WaterMove);
    }
}
