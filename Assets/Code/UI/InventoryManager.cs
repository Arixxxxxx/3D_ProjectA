using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject inventory_Obj;
    [SerializeField] Slot[] slots;
    [SerializeField] Transform slotHolder;
    Inventory inven;
    private bool inventory;
    public bool isInventoryOpen { get { return inventory; } set { inventory = value; } }
    bool bdown;

    private void Start()
    {
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inven = Inventory.Inst;
        inven.onSlotCountChange += SlotChange;
    }

    private void SlotChange(int value)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(i < inven.SlotCount)
            {
                slots[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                slots[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    private void Update()
    {
        F_InventoryOnOFF();
        Cur_InventorySituation();
    }

    
    private void F_InventoryOnOFF()
    {
        bdown = Input.GetKeyDown(KeyCode.B);

        if (bdown)
        {
            isInventoryOpen = !isInventoryOpen;
        }
            
    }
    
    private void Cur_InventorySituation()
    {
        if (inventory && inventory_Obj.activeSelf  == false)
        {
            inventory_Obj.SetActive(true);
            GameManager.Inst.F_SetScreenCursorOnOFF(true);
            GameManager.Inst.F_SetMouseScrrenRotationStop(true);
        }
        else if(inventory == false && inventory_Obj.activeSelf)
        {
            inventory_Obj.SetActive(false);
            GameManager.Inst.F_SetScreenCursorOnOFF(false);
            GameManager.Inst.F_SetMouseScrrenRotationStop(false);
        }
    }

    public void B_AddSlot()
    {
        inven.SlotCount++;
    }

}
