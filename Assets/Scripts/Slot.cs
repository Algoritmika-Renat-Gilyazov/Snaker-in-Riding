using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    protected Item item;

    public Image icon;

    public bool isEmpty()
    {
        if (item == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetItem(Item item)
    {
        this.item = item;
    }

    public Item GetItem()
    {
        return item;
    }

    public bool isItem(Item item)
    {
        return this.item.id == item.id;
    }

    public void Lose()
    {
        if (!isEmpty())
        {
            item = null;
        }
    }

    public void UseItem()
    {
        if (!isEmpty())
        {
            item.Use();
        }
    }
}