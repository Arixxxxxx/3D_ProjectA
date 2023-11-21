using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Inst;
    #region

    public delegate void OnSlotCountChange(int value);
    public OnSlotCountChange onSlotCountChange;

    private void Awake()
    {
        if(Inst != null)
        {
            Destroy(this);
            return;
        }
        Inst = this;
    }
    #endregion

    private int slotCount;
    public int SlotCount
    {
        get => slotCount;
        set
        {
            slotCount = value;
            onSlotCountChange.Invoke(slotCount);
        }
    }

    void Start()
    {
        SlotCount = 6;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
