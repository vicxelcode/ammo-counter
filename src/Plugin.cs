using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace AmmoCounter;

[BepInPlugin(Guid, Name, Version)]
public class Plugin : BaseUnityPlugin
{
    public const string Guid = "vicxelcode.AmmoCounter";
    public const string Name = "Ammo Counter";
    public const string Version = "1.0.0";

    internal static ManualLogSource Log;

    private void Awake()
    {
        Log = Logger;
        Settings.Bind(Config);
        new Harmony(Guid).PatchAll(typeof(Plugin).Assembly);
        Log.LogInfo($"{Name} {Version} cargado");
    }
}
