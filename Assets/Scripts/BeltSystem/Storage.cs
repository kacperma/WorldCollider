using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
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

    public override bool CanGiveItem(BeltItemDataSO beltItemData)
    {
        if (this.beltItemData == null)
            return false;
        return this.beltItemData == beltItemData && this.stackSize > 0;
    }

    public override bool CanPlaceItem(BeltItemDataSO beltItemData)
    {
        if (this.beltItemData == null)
            return true;
        return this.beltItemData == beltItemData && this.stackSize < 100;
    }

    public override bool CanReceiveItem(BeltItemDataSO beltItemData)
    {
        return CanPlaceItem(beltItemData);
    }

    public override void FreeSpace()
    {
        // resets gameobject and lowers space available
        if (this.stackSize == 0)
        {
            this.beltItemData = null;
            this.beltItemObject = null;
            return;
        }
        this.stackSize--;
    }

    public override BeltItemDataSO GetItemData()
    {
        // if storage is empty
        if (this.stackSize == 0)
            return null;
        return beltItemData;
        
    }

    public override GameObject GetItemObject()
    {
        this.beltItemObject = Instantiate(this.beltItemData.Prefab, this.GetItemPosition(), Quaternion.identity);
        this.beltItemObject.name = this.beltItemData.Name;
        return this.beltItemObject;
    }

    public override void ReceiveItem(BeltItemDataSO beltItemData, GameObject beltItemObject)
    {
        if (this.beltItemData == null)
        {
            this.beltItemData = beltItemData;
        }
        this.stackSize++;
        Destroy(beltItemObject);
    }
}
