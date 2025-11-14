using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Slot[] slots;

    public GameObject selector;

    public TextMeshProUGUI itemName;
    public int selectedSlot = 0;

    // Добавить предмет, если есть место, иначе заменить в выбранной ячейке
    public bool AddItem(Item item, int selectedSlot)
    {
        // Сначала ищем свободный слот
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].isEmpty())
            {
                slots[i].SetItem(item);
                Debug.Log($"Добавлен предмет {item.id} в слот {i + 1}");
                slots[i].icon.sprite = item.sprite;
                itemName.text = item.itemName;
                return true;
            }
        }
        // Если нет свободных слотов — заменяем в выбранном
        if (selectedSlot >= 0 && selectedSlot < slots.Length)
        {
            Debug.Log($"Инвентарь заполнен! Заменён предмет {slots[selectedSlot].GetItem().id} на {item.id} в слоте {selectedSlot + 1}");
            slots[selectedSlot].SetItem(item);
            slots[selectedSlot].icon.sprite = item.sprite;
            itemName.text = item.itemName;
            return true;
        }
        Debug.Log("Некорректный индекс выбранного слота!");
        return false;
    }

    // Удалить предмет из слота
    public void RemoveItem(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slots.Length)
        {
            Debug.Log($"Удалён предмет {slots[slotIndex]} из слота {slotIndex + 1}");
            slots[slotIndex].Lose();
        }
        if(slotIndex == selectedSlot)
        {
            itemName.text = "Item";
        }
    }

    // Получить предмет из слота
    public Item GetItem(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slots.Length)
        {
            return slots[slotIndex].GetItem();
        }
        return null;
    }

    // Проверить, есть ли предмет в инвентаре
    public bool HasItem(Item item)
    {
        foreach (var slot in slots)
        {
            if (slot.isItem(item)) return true;
        }
        return false;
    }

    public void Scroll()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            Debug.Log("Scroll Up");
            selectedSlot += 1;
            if (selectedSlot >= slots.Length)
            {
                selectedSlot = 0; // Циклический переход к первому слоту
            }
            else if (selectedSlot < 0)
            {
                selectedSlot = slots.Length - 1; // Циклический переход к последнему слоту
            }
            Debug.Log("Selected Slot: " + (selectedSlot + 1));
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            Debug.Log("Scroll Down");
            selectedSlot -= 1;
            if (selectedSlot >= slots.Length)
            {
                selectedSlot = 0; // Циклический переход к первому слоту
            }
            else if (selectedSlot < 0)
            {
                selectedSlot = slots.Length - 1; // Циклический переход к последнему слоту
            }
            Debug.Log("Selected Slot: " + (selectedSlot + 1));
        }
        Vector3 pos = selector.transform.position;
        pos.x = slots[selectedSlot].transform.position.x;
        selector.transform.position = pos;
        if (!slots[selectedSlot].isEmpty())
        {
            itemName.text = slots[selectedSlot].GetItem().itemName;
        }
        else
        {
            itemName.text = "Item";
        }
    }
    
    public void UseSelectedItem()
    {
        if (!slots[selectedSlot].isEmpty())
        {
            slots[selectedSlot].UseItem();
            if (slots[selectedSlot].isEmpty())
            {
                itemName.text = "Item";
                slots[selectedSlot].icon.sprite = null;
                
            }
        }
    }
}