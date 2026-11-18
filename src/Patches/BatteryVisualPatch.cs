using System;
using HarmonyLib;

namespace AmmoCounter.Patches;

/// <summary>Solo lee: cada fotograma pone al día el número de cada indicador de batería del interfaz.</summary>
[HarmonyPatch(typeof(BatteryVisualLogic), nameof(BatteryVisualLogic.Update))]
internal static class BatteryVisualPatch
{
    private static bool failed;

    private static void Postfix(BatteryVisualLogic __instance)
    {
        if (failed) return;
        try
        {
            AmmoLabel.Tick(__instance);
        }
        catch (Exception e)
        {
            // Un fallo del contador no debe repetirse cada fotograma ni romper el HUD.
            failed = true;
            Plugin.Log.LogError($"Contador desactivado: {e}");
        }
    }
}
