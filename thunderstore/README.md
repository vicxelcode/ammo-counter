# Ammo Counter

See how many shots you have left at a glance: `7/12` on every inventory slot that holds a gun, and next to the
battery of the gun in your hand. Bars are hard to count when a gun has a lot of them; a number is not.

- **Inventory slots.** Above the battery bars of each slot. Extra slots added by other mods (for example
  ExtraInventorySlots) get it too.
- **Held item.** Next to the battery shown while you hold a gun.
- **Only reads.** It doesn't change your ammo, so it works with any ammo or balance mod, and the number follows
  whatever they set.
- Optionally, the battery percentage of drones, staffs and other battery items.

## Configuration

`BepInEx/config/vicxelcode.AmmoCounter.cfg`:

| Section | Setting | Default | What it does |
|---|---|---|---|
| General | `Items` | `Guns` | `AllBatteryItems` also shows the battery % of other items |
| Inventory | `Enabled`, `Scale`, `OffsetX`, `OffsetY` | `true`, `1`, `0`, `0` | The number on the inventory slots |
| Held item | `Enabled`, `Scale`, `OffsetX`, `OffsetY` | `true`, `1`, `0`, `0` | The number next to the gun in your hand |

Client-side: only you need it, and it doesn't matter whether the host has it.

Not affiliated with semiwork.

## Support

If you enjoy the mod, you can [buy me a coffee](https://buymeacoffee.com/vicxelcode). Thanks!
