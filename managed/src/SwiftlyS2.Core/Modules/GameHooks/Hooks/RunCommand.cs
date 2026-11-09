using System.Runtime.InteropServices;
using SwiftlyS2.Core.Events;
using SwiftlyS2.Core.Memory;
using SwiftlyS2.Shared.GameHooks;
using SwiftlyS2.Shared.Memory;
using SwiftlyS2.Shared.Misc;

namespace SwiftlyS2.Core.GameHooks;

internal static partial class GameHooksPublisher
{
    private delegate nint CPlayerMovementServicesRunCommand( nint pMovementServices, nint pUserCmd );

    private static nint runCommandOriginal;

    internal static unsafe Guid HookRunCommand()
    {
        if (_core == null) throw new InvalidOperationException("GameHooksCore is not initialized.");

        var offset = _core.GameData.GetOffset("CPlayer_MovementServices::RunCommand");
        if (offset < 0)
            throw new InvalidOperationException("Failed to find offset for CPlayer_MovementServices::RunCommand.");

        var runCommand = (UnmanagedFunction<CPlayerMovementServicesRunCommand>)_core.Memory.GetUnmanagedFunctionByVTable<CPlayerMovementServicesRunCommand>(_core.Memory.GetVTableAddress(Library.Server, "CPlayer_MovementServices")!.Value, offset);
        return runCommand.AddHook((nint)(delegate* unmanaged<nint, nint, nint>)&RunCommandDetour, static original => Volatile.Write(ref runCommandOriginal, original));
    }

    [UnmanagedCallersOnly]
    private static unsafe nint RunCommandDetour( nint pMovementServices, nint pUserCmd )
    {
        var original = (delegate* unmanaged<nint, nint, nint>)Volatile.Read(ref runCommandOriginal);

        var dummy = _pawnComponentPool.Rent();
        dummy.DangerousSetHandle(pMovementServices);
        var player = dummy.ToPlayer();
        _pawnComponentPool.Return(dummy);
        if (player == null) return original(pMovementServices, pUserCmd);

        var userCmdImpl = _userCmdPool.Rent();
        userCmdImpl.Address = pUserCmd;

        var preCtx = new RunCommandMovementPreContext {
            Params = new RunCommandMovementParams {
                Player = player,
                UserCmd = userCmdImpl
            }
        };

        if (EventPublisher.ListensToRunCommand)
        {
            using var ev = new OnMovementServicesRunCommandHookEvent {
                MovementServices = preCtx.Params.Player.PlayerPawn!.MovementServices!,
                ButtonState = preCtx.Params.UserCmd.ButtonState,
                UserCmdPB = preCtx.Params.UserCmd.CSGOUserCmd
            };
            EventPublisher.InvokeOnMovementServicesRunCommandHook(ev);
        }

        InvokeRunCommandPre(ref preCtx);
        if (preCtx.HookResult == HookResult.Stop || preCtx.HookResult == HookResult.CancelOriginal)
        {
            userCmdImpl.Address = 0;
            _userCmdPool.Return(userCmdImpl);
            return 0;
        }

        var result = original(pMovementServices, pUserCmd);

        var postCtx = new RunCommandMovementPostContext { Params = preCtx.Params };

        InvokeRunCommandPost(ref postCtx);

        userCmdImpl.Address = 0;
        _userCmdPool.Return(userCmdImpl);

        return result;
    }

    internal static Guid UnhookRunCommand()
    {
        if (_core == null) throw new InvalidOperationException("GameHooksCore is not initialized.");

        if (hookIds.TryGetValue(HookListener.RunCommand, out var hookId))
        {
            var offset = _core.GameData.GetOffset("CPlayer_MovementServices::RunCommand");
            if (offset < 0)
                throw new InvalidOperationException("Failed to find offset for CPlayer_MovementServices::RunCommand.");

            var runCommand = _core.Memory.GetUnmanagedFunctionByVTable<CPlayerMovementServicesRunCommand>(_core.Memory.GetVTableAddress(Library.Server, "CPlayer_MovementServices")!.Value, offset);
            runCommand.RemoveHook(hookId);
            return hookId;
        }
        else return Guid.Empty;
    }

    internal static void InvokeRunCommandPre( ref RunCommandMovementPreContext ctx )
    {
        var services = Volatile.Read(ref preServices[(int)HookListener.RunCommand]);
        for (var i = 0; i < services.Length; i++)
        {
            services[i].InvokeRunCommandPre(ref ctx);
            if (ctx.HookResult == HookResult.Stop || ctx.HookResult == HookResult.Handled) return;
        }
    }

    internal static void InvokeRunCommandPost( ref RunCommandMovementPostContext ctx )
    {
        var services = Volatile.Read(ref postServices[(int)HookListener.RunCommand]);
        for (var i = 0; i < services.Length; i++)
        {
            services[i].InvokeRunCommandPost(ref ctx);
            if (ctx.HookResult == HookResult.Stop || ctx.HookResult == HookResult.Handled) return;
        }
    }
}
