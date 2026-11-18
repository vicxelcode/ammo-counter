using BepInEx.Configuration;

namespace AmmoCounter;

public enum CounterItems { Guns, AllBatteryItems }

internal static class Settings
{
    public static ConfigEntry<CounterItems> Items;
    public static Place Inventory;
    public static Place Hand;

    /// <summary>Un sitio donde sale el número: los huecos del inventario o la batería del objeto en la mano.</summary>
    internal sealed class Place
    {
        public ConfigEntry<bool> Enabled;
        public ConfigEntry<float> Scale;
        public ConfigEntry<float> OffsetX;
        public ConfigEntry<float> OffsetY;
    }

    public static void Bind(ConfigFile cfg)
    {
        Items = cfg.Bind("General", "Items", CounterItems.Guns,
            "Guns: shots left on guns only. AllBatteryItems: also the battery percentage of drones, staffs and other battery items.");
        Inventory = BindPlace(cfg, "Inventory", "Show the shots left on each inventory slot (extra slots added by other mods included).");
        Hand = BindPlace(cfg, "Held item", "Show the shots left next to the battery of the item you are holding.");
    }

    private static Place BindPlace(ConfigFile cfg, string section, string what) => new()
    {
        Enabled = cfg.Bind(section, "Enabled", true, what),
        Scale = cfg.Bind(section, "Scale", 1f,
            new ConfigDescription("Size of the number.", new AcceptableValueRange<float>(0.5f, 2.5f))),
        OffsetX = cfg.Bind(section, "OffsetX", 0f,
            new ConfigDescription("Moves the number right (or left if negative), in HUD pixels.", new AcceptableValueRange<float>(-200f, 200f))),
        OffsetY = cfg.Bind(section, "OffsetY", 0f,
            new ConfigDescription("Moves the number up (or down if negative), in HUD pixels.", new AcceptableValueRange<float>(-200f, 200f))),
    };
}
