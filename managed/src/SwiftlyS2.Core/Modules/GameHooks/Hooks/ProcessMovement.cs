using System.Runtime.InteropServices;
using SwiftlyS2.Core.Memory;
using SwiftlyS2.Shared.GameHooks;
using SwiftlyS2.Shared.Misc;

namespace SwiftlyS2.Core.GameHooks;

internal static partial class GameHooksPublisher
{
    private delegate void CCSPlayerMovementServicesProcessMovement( nint movementServices, nint moveData );

    private static nint processMovementOriginal;

    internal static unsafe Guid HookProcessMovement()
    {
        if (_core == null) throw new InvalidOperationException("GameHooksCore is not initialized.");

        var ProcessMovementPtr = _core.GameData.GetSignature("CCSPlayer_MovementServices::ProcessMovement");
        if (ProcessMovementPtr == 0)
            throw new InvalidOperationException("Failed to find signature for CCSPlayer_MovementServices::ProcessMovement.");

        var ProcessMovementUmanagedFunction = (UnmanagedFunction<CCSPlayerMovementServicesProcessMovement>)_core.Memory.GetUnmanagedFunctionByAddress<CCSPlayerMovementServicesProcessMovement>(ProcessMovementPtr);
        return ProcessMovementUmanagedFunction.AddHook((nint)(delegate* unmanaged<nint, nint, void>)&ProcessMovementDetour, static original => Volatile.Write(ref processMovementOriginal, original));
    }

    [UnmanagedCallersOnly]
    private static unsafe void ProcessMovementDetour( nint movementServices, nint moveData )
    {
        var original = (delegate* unmanaged<nint, nint, void>)Volatile.Read(ref processMovementOriginal);

        var dummy = _pawnComponentPool.Rent();
        dummy.DangerousSetHandle(movementServices);
        var player = dummy.ToPlayer();
        _pawnComponentPool.Return(dummy);
        if (player == null) { original(movementServices, moveData); return; }

        var moveDataImpl = _moveDataPool.Rent();
        moveDataImpl.Address = moveData;

        var preCtx = new ProcessMovementMovementPreContext {
            Params = new ProcessMovementMovementParams {
                Player = player,
                MoveData = moveDataImpl
            }
        };

        InvokeProcessMovementPre(ref preCtx);
        if (preCtx.HookResult == HookResult.Stop || preCtx.HookResult == HookResult.CancelOriginal)
        {
            moveDataImpl.Address = 0;
            _moveDataPool.Return(moveDataImpl);
            return;
        }

        original(movementServices, moveData);

        var postCtx = new ProcessMovementMovementPostContext { Params = preCtx.Params };

        InvokeProcessMovementPost(ref postCtx);

        moveDataImpl.Address = 0;
        _moveDataPool.Return(moveDataImpl);
    }

    internal static Guid UnhookProcessMovement()
    {
        if (_core == null) throw new InvalidOperationException("GameHooksCore is not initialized.");

        if (hookIds.TryGetValue(HookListener.ProcessMovement, out var hookId))
        {
            var ProcessMovementPtr = _core.GameData.GetSignature("CCSPlayer_MovementServices::ProcessMovement");
            if (ProcessMovementPtr == 0)
                throw new InvalidOperationException("Failed to find signature for CCSPlayer_MovementServices::ProcessMovement.");

            var ProcessMovementUmanagedFunction = _core.Memory.GetUnmanagedFunctionByAddress<CCSPlayerMovementServicesProcessMovement>(ProcessMovementPtr);

            ProcessMovementUmanagedFunction.RemoveHook(hookId);
            return hookId;
        }
        else return Guid.Empty;
    }

    internal static void InvokeProcessMovementPre( ref ProcessMovementMovementPreContext ctx )
    {
        var services = Volatile.Read(ref preServices[(int)HookListener.ProcessMovement]);
        for (var i = 0; i < services.Length; i++)
        {
            services[i].InvokeProcessMovementPre(ref ctx);
            if (ctx.HookResult == HookResult.Stop || ctx.HookResult == HookResult.Handled) return;
        }
    }

    internal static void InvokeProcessMovementPost( ref ProcessMovementMovementPostContext ctx )
    {
        var services = Volatile.Read(ref postServices[(int)HookListener.ProcessMovement]);
        for (var i = 0; i < services.Length; i++)
        {
            services[i].InvokeProcessMovementPost(ref ctx);
            if (ctx.HookResult == HookResult.Stop || ctx.HookResult == HookResult.Handled) return;
        }
    }
}
