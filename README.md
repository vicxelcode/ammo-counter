# Ammo Counter

Mod de R.E.P.O. (BepInEx 5) que enseña los tiros que quedan en número: en cada hueco del inventario y junto a
la batería del arma que llevas en la mano. Nació porque ningún mod de Thunderstore lo hacía:
`DarkSpider-ExtraInventorySlots` 1.2.1 solo pone en los huecos la etiqueta de la tecla (`SlotLabelPainter`) y las
barras del juego (comprobado descompilándolo el 2026-11-16).

El README para jugadores (en inglés) está en [thunderstore/README.md](thunderstore/README.md).

## Cómo funciona

- **La munición es la batería.** Cada arma tiene `ItemBattery.batteryBars` barras y cada disparo gasta una
  (`ItemGun.batteryDrainFullBar` + `ItemBattery.RemoveFullBar`). Tiros que quedan = `batteryLifeInt`; el host
  lo sincroniza con todos, así que el número es el real también en los clientes.
- **Dónde sale.** Todos los indicadores de batería del interfaz son `BatteryVisualLogic` con `inUI`: uno por
  hueco del inventario (`InventorySpot`) y el del objeto en la mano (`BatteryUI`). Un postfix en
  `BatteryVisualLogic.Update` le cuelga a cada uno un `AmmoLabel` (un `TextMeshProUGUI`), que aparece, se esconde
  y rebota con él. Los huecos que añaden otros mods copiando `InventorySpot` también lo llevan.
- **Solo lee.** No cambia la munición: vale con cualquier mod de munición o equilibrio y no hace falta que lo
  tenga el host.

## Estructura

```
src/Plugin.cs                    entrada BepInEx
src/Settings.cs                  configuración (BepInEx/config/vicxelcode.AmmoCounter.cfg)
src/AmmoLabel.cs                 el número de cada indicador
src/Patches/BatteryVisualPatch.cs  engancha AmmoLabel a BatteryVisualLogic.Update
thunderstore/                    README e icono del paquete (el manifest lo genera tcli)
```

## Desarrollo

Requisitos: .NET SDK (8 o superior) y R.E.P.O. instalado; otra ruta con `REPO_DIR=...` o `-p:RepoDir=...`.

- `bash scripts/dev-run.sh` compila en Debug, lo copia a un perfil limpio (`../../.e2e/ammo-counter-profile`) y
  lanza el juego. Antes, la ventana al escritorio «Claude» (`scripts/claude-desktop.ps1 -Names REPO`).
- `bash scripts/package.sh` compila en Release y construye el zip de Thunderstore en `dist/` con `tcli`
  (`dotnet tool install -g tcli`) a partir de `thunderstore.toml`, fuente única de nombre, versión,
  dependencias y categorías. La versión va en `thunderstore.toml` y en `Plugin.Version`; el script
  comprueba que coinciden. Ese zip se puede importar en un pack de REPO Party como mod local para probarlo
  con amigos antes de publicarlo.
- `bash scripts/publish.sh` lo construye y lo sube a Thunderstore (equipo `vicxelcode`, comunidad `repo`).
  Necesita el token de la cuenta de servicio del equipo en `../../.keys/thunderstore.env` como
  `TCLI_AUTH_TOKEN=...`; nunca se imprime.

No afiliado a semiwork.
