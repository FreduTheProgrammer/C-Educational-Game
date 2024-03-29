using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{

    public class InventoryItem
    {
        public string itemName;
        public int itemID;
        public Vector3 itemPosition;

        public InventoryItem(string name, int id, Vector3 pos)
        {
            itemName = name;
            itemID = id;
            itemPosition = pos;

        }
    }

    public List<InventoryItem> items = new List<InventoryItem>();
    public Transform itemsParent; // Referencja do rodzica, gdzie będą przechowywane elementy UI reprezentujące przedmioty
    public GameObject itemPrefab; // Prefabrykat przedmiotu w ekwipunku

    // Metoda do aktualizacji ekwipunku w interfejsie użytkownika
    public void UpdateInventoryUI()
    {
        // Usunięcie wszystkich dzieci obiektu przechowującego przedmioty
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        // Dodanie nowych obiektów UI reprezentujących przedmioty z ekwipunku
        foreach (var item in items)
        {
            GameObject newItem = Instantiate(itemPrefab, itemsParent);
            itemPrefab.transform.position = item.itemPosition;
            
            // Tutaj możesz ustawiać odpowiednie wartości tekstów, ikon itp. w zależności od przedmiotu
            // np. newItem.GetComponentInChildren<Text>().text = item.itemName;
        }
    }

    public void AddItem(InventoryItem newItem)
    {
        items.Add(newItem);
        UpdateInventoryUI();
    }

    public void RemoveItem(InventoryItem itemToRemove)
    {
        if (items.Contains(itemToRemove))
        {
            items.Remove(itemToRemove);
            UpdateInventoryUI();
        }
    }

    // Przykład użycia:
    void Start()
    {
        InventoryItem sword = new InventoryItem("Miecz", 1, new Vector3(15,-15,0));
        InventoryItem potion = new InventoryItem("Mikstura zdrowia", 2, new Vector3(45, -15, 0));

        AddItem(sword);
        AddItem(potion);

        
    }
}
