using SwiftlyS2.Shared.GameHooks;

namespace SwiftlyS2.Core.GameHooks;

internal sealed class SimulateUserCommandsHook : ISimulateUserCommandsHook
{
    private event OnSimulateUserCommandsPreDelegate? _Pre;
    private event OnSimulateUserCommandsPostDelegate? _Post;

    public event OnSimulateUserCommandsPreDelegate Pre
    {
        add
        {
            if (_Pre == null) GameHooksPublisher.AddHookListener(HookListener.SimulateUserCommands);
            _Pre += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.SimulateUserCommands);
        }
        remove
        {
            _Pre -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.SimulateUserCommands);
            if (_Pre == null) GameHooksPublisher.RemoveHookListener(HookListener.SimulateUserCommands);
        }
    }

    public event OnSimulateUserCommandsPostDelegate Post
    {
        add
        {
            if (_Post == null) GameHooksPublisher.AddHookListener(HookListener.SimulateUserCommands);
            _Post += value;
            GameHooksPublisher.RefreshSnapshots(HookListener.SimulateUserCommands);
        }
        remove
        {
            _Post -= value;
            GameHooksPublisher.RefreshSnapshots(HookListener.SimulateUserCommands);
            if (_Post == null) GameHooksPublisher.RemoveHookListener(HookListener.SimulateUserCommands);
        }
    }

    public void InvokePre(ref SimulateUserCommandsPreContext ctx) => _Pre?.Invoke(ref ctx);
    public void InvokePost(ref SimulateUserCommandsPostContext ctx) => _Post?.Invoke(ref ctx);

    public bool HasPreListeners => _Pre != null;
    public bool HasPostListeners => _Post != null;

    public void UnregisterListeners()
    {
        if (_Pre != null) GameHooksPublisher.RemoveHookListener(HookListener.SimulateUserCommands);
        if (_Post != null) GameHooksPublisher.RemoveHookListener(HookListener.SimulateUserCommands);
    }
}
