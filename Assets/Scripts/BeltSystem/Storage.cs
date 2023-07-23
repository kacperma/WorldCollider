using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Storage : Logistic
{
    private static int _storageID = 0;
    [SerializeField]
    protected int stackSize = 100;

    protected override int _Start()
    {
        return _storageID++;
    }
    
    public override bool CanGiveItem(BeltItem beltItem)
    {
        if (this.beltItem == null)
            return false;
        return this.beltItem.ID == beltItem.ID && this.stackSize > 0;
    }

    public override bool CanPlaceItem(BeltItem beltItem)
    {
        if (this.beltItem == null)
            return true;
        return this.beltItem.ID == beltItem.ID && this.stackSize < 100;
    }

    public override bool CanTakeItem(BeltItem beltItem)
    {
        return CanPlaceItem(beltItem);
    }

    public override void FreeSpace()
    {
        this.stackSize--;
    }

    public override BeltItem GetItem()
    {
        Instantiate(this.beltItem.item);
        return beltItem;
    }

    public override void ReceiveItem(BeltItem beltItem)
    {
        this.beltItem = new BeltItem(beltItem.ID);
        Destroy(beltItem.item);
        this.stackSize++;
    }

}
