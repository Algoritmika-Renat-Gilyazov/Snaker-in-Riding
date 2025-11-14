using UnityEngine;
using System.Collections;

public class TestItem : Item { 
    public override void Use() {
        Debug.Log($"Used Test Item");
        FindAnyObjectByType<Player>().Heal(10);
        FindAnyObjectByType<Inventory>().RemoveItem(FindAnyObjectByType<Inventory>().selectedSlot);
    }

    private void Start()
    {
        itemName = "Test Item";
        id = "snaker:test_item";
    }
}