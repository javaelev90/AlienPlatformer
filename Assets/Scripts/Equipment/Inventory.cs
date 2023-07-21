using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IStateful
{
    private InventoryState inventoryState;
    private IItem[] inventory;
    private int inventorySize = 2;
    private IItem selectedItem;
    private int selectedItemIndex = 0;
    private int previousSelectedItemIndex;
    [SerializeField] private GameObject selectedItemPosition;
    [SerializeField] private UI_Inventory UI_Inventory;
    private Dictionary<int, IItem> itemsTouching;
    private bool isInventoryActive = true;

    void Start()
    {
        previousSelectedItemIndex = selectedItemIndex;
        inventory = new IItem[inventorySize];
        itemsTouching = new Dictionary<int, IItem>();
    }

    void Update()
    {
        if(isInventoryActive){
            int keyPressed = GetKeyPressed();

            if (keyPressed != -1 && inventory.Length > keyPressed)
            {
                previousSelectedItemIndex = selectedItemIndex;
                selectedItemIndex = keyPressed;
                if (selectedItem != null)
                {
                    if (selectedItem is IEquipable) ((IEquipable)selectedItem).UnEquip();
                    selectedItem.InActivate();
                }
                SelectItemInSlot(selectedItemIndex);
                if (selectedItem is IEquipable) ((IEquipable)selectedItem).Equip();
            }

            if (selectedItem != null)
            {
                selectedItem.Activate();
                // if (Input.GetKeyDown(KeyCode.F))
                // {
                //     selectedItem.InActivate();
                //     RemoveFromInventory(selectedItem);
                // }
            }

            if (itemsTouching.Count > 0)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    foreach (KeyValuePair<int, IItem> kvp in itemsTouching)
                    {
                        AddToInventory(kvp.Value.PickUp());
                        break;
                    }
                }
            }
        }
        
    }

    public bool ContainsItem(string itemName)
    {
        foreach (IItem item in inventory)
        {
            if (item != null && item.GetItemName() == itemName)
            {
                return true;
            }
        }
        return false;
    }

    public IItem GetItem(string itemName)
    {
        foreach (IItem item in inventory)
        {
            if (item != null && item.GetItemName() == itemName)
            {
                return item;
            }
        }
        return null;
    }

    private void SelectItemInSlot(int slotIndex)
    {
        if (inventory[slotIndex] != null) {
            UI_Inventory.DeselectItemInSlot(previousSelectedItemIndex);
            selectedItem = inventory[slotIndex];
            UI_Inventory.SelectItemInSlot(selectedItemIndex);
        }
    }

    private void AddToInventory(IItem item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                UI_Inventory.AddItemToSlot(item.getItemSprite().texture, i);
                inventory[i] = item;
                break;
            }
        }
    }

    private void RemoveFromInventory(IItem item)
    {
        item.Drop();
        selectedItem = null;
        inventory[selectedItemIndex] = null;
        UI_Inventory.RemoveItemFromSlot(selectedItemIndex);
        UI_Inventory.DeselectItemInSlot(selectedItemIndex);
    }

    private int GetKeyPressed()
    {
        int keyPressed = -1;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            keyPressed = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            keyPressed = 1;
        }
        return keyPressed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            itemsTouching.Add(collision.gameObject.GetInstanceID(), collision.gameObject.GetComponent<IItem>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            itemsTouching.Remove(collision.gameObject.GetInstanceID());
        }
    }

    public void SaveState()
    {
        inventoryState = new InventoryState()
        {
            selectedItemIndex = this.selectedItemIndex,
            previousSelectedItemIndex = this.previousSelectedItemIndex,
        };
        if(inventory == null) return;
        foreach(IItem item in inventory)
        {
            if (item != null && item is IStateful)
            {
                ((IStateful)item).SaveState();
            }
        }
    }

    public void ToggleInventoryActive(){
        isInventoryActive = !isInventoryActive;
    }
    public void LoadSavedState()
    {
        selectedItemIndex = inventoryState.selectedItemIndex;
        previousSelectedItemIndex = inventoryState.previousSelectedItemIndex;
        if(inventory == null) return;
        foreach (IItem item in inventory)
        {
            if (item != null && item is IStateful)
            {
                ((IStateful)item).LoadSavedState();
            }
        }
    }

    private class InventoryState
    {
        public int selectedItemIndex = 0;
        public int previousSelectedItemIndex;
    }
}


