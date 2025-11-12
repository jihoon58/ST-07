using ST07.Player;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public Text quantityText;

    public void Set(ItemStack stack)
    {
        if (stack == null || stack.item == null)
        {
            Clear();
            return;
        }

        icon.enabled = true;
        icon.sprite = stack.item.icon;

        if (stack.item.maxStack > 1 && stack.quantity > 1)
            quantityText.text = stack.quantity.ToString();
        else
            quantityText.text = "";
    }

    public void Clear()
    {
        if (icon != null)
        {
            icon.enabled = false;
            icon.sprite = null;
        }

        if (quantityText != null)
            quantityText.text = "";
    }
}