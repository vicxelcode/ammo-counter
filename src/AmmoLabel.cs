using System.Linq;
using TMPro;
using UnityEngine;

namespace AmmoCounter;

/// <summary>
/// El número de tiros de un indicador de batería del interfaz (<c>BatteryVisualLogic</c> con <c>inUI</c>):
/// el de cada hueco del inventario (<c>InventorySpot</c>, también los que añaden otros mods copiándolo) y
/// el del objeto en la mano (<c>BatteryUI</c>). Cuelga del propio indicador, así que aparece, se esconde,
/// rebota y se mueve con él. Solo lee: no cambia la munición.
/// </summary>
internal sealed class AmmoLabel : MonoBehaviour
{
    private const float BaseFontSize = 20f;

    private static TMP_FontAsset font;

    private BatteryVisualLogic visual;
    private Settings.Place place;
    private bool inSlot;
    private TextMeshProUGUI text;
    private string shown;
    private ItemBattery battery;
    private bool isGun;

    public static void Tick(BatteryVisualLogic visual)
    {
        if (!visual.inUI) return; // los indicadores que flotan sobre los objetos del mundo no se tocan
        var label = visual.GetComponent<AmmoLabel>();
        if (!label)
        {
            label = visual.gameObject.AddComponent<AmmoLabel>();
            label.Init(visual);
        }
        label.Refresh();
    }

    private void Init(BatteryVisualLogic owner)
    {
        visual = owner;
        inSlot = owner.GetComponentInParent<InventorySpot>();
        place = inSlot ? Settings.Inventory : Settings.Hand;

        var go = new GameObject("AmmoCounter", typeof(RectTransform));
        go.transform.SetParent(owner.transform, false);
        text = go.AddComponent<TextMeshProUGUI>();
        text.font = Font();
        text.raycastTarget = false;
        text.textWrappingMode = TextWrappingModes.NoWrap;
        text.overflowMode = TextOverflowModes.Overflow;
        text.outlineWidth = 0.2f;
        text.outlineColor = new Color32(0, 0, 0, 200);

        var rt = text.rectTransform;
        rt.sizeDelta = new Vector2(120f, 30f);
        if (inSlot)
        {
            // Hueco del inventario: encima de las barras y centrado (a los lados están los huecos vecinos).
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 1f);
            rt.pivot = new Vector2(0.5f, 0f);
            text.alignment = TextAlignmentOptions.Bottom;
        }
        else
        {
            // Objeto en la mano: a la derecha de la batería.
            rt.anchorMin = rt.anchorMax = new Vector2(1f, 0.5f);
            rt.pivot = new Vector2(0f, 0.5f);
            text.alignment = TextAlignmentOptions.MidlineLeft;
        }

        if (owner.transform is RectTransform r)
            Plugin.Log.LogDebug($"Indicador {(inSlot ? "de hueco" : "en la mano")}: {r.rect.size}, escala {r.lossyScale}");
    }

    private void Refresh()
    {
        var value = place.Enabled.Value ? Value(visual.itemBattery) : null;
        if (value == null)
        {
            if (text.gameObject.activeSelf) text.gameObject.SetActive(false);
            return;
        }
        if (value != shown)
        {
            text.text = value;
            shown = value;
        }
        text.fontSize = BaseFontSize * place.Scale.Value;
        var gap = inSlot ? new Vector2(0f, 2f) : new Vector2(6f, 0f);
        text.rectTransform.anchoredPosition = gap + new Vector2(place.OffsetX.Value, place.OffsetY.Value);
        if (!text.gameObject.activeSelf) text.gameObject.SetActive(true);
    }

    private string Value(ItemBattery item)
    {
        if (!item) return null;
        if (item != battery)
        {
            battery = item;
            isGun = item.GetComponent<ItemGun>();
        }

        if (isGun)
        {
            // Cada disparo gasta una barra (ItemGun.batteryDrainFullBar): barras que quedan / barras = tiros.
            var max = item.batteryBars;
            var left = Mathf.Clamp(item.batteryLifeInt, 0, max);
            var color = left == 0 ? "#FF4D4F" : "#FFFFFF";
            return $"<color={color}>{left}</color><size=60%><color=#FFFFFFAA>/{max}</color></size>";
        }

        if (Settings.Items.Value != CounterItems.AllBatteryItems) return null;
        return $"{Mathf.CeilToInt(Mathf.Clamp(item.batteryLife, 0f, 100f))}<size=60%><color=#FFFFFFAA>%</color></size>";
    }

    // Teko es la fuente estrecha del HUD del juego.
    private static TMP_FontAsset Font()
    {
        if (font) return font;
        font = Resources.FindObjectsOfTypeAll<TMP_FontAsset>()
                   .FirstOrDefault(f => f && f.name.StartsWith("Teko", System.StringComparison.OrdinalIgnoreCase))
               ?? TMP_Settings.defaultFontAsset;
        return font;
    }
}
