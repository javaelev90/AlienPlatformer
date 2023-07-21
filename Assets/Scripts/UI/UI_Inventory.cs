using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Inventory : MonoBehaviour
{
    [SerializeField] List<GameObject> itemSlots;
    [SerializeField] List<GameObject> itemSlotSelected;
    [SerializeField] Texture2D defaultItemImage;

    void Start()
    {
        foreach (GameObject image in itemSlotSelected)
        {
            image.SetActive(false);
        }
    }

    public void AddItemToSlot(Texture2D itemImage, int slotIndex)
    {
        itemSlots[slotIndex].GetComponent<RawImage>().texture = itemImage;
    }

    public void RemoveItemFromSlot(int slotIndex)
    {
        itemSlots[slotIndex].GetComponent<RawImage>().texture = defaultItemImage;
    }

    public void SelectItemInSlot(int slotIndex)
    {
        itemSlotSelected[slotIndex].SetActive(true);
    }

    public void DeselectItemInSlot(int slotIndex)
    {
        itemSlotSelected[slotIndex].SetActive(false);
    }
}
