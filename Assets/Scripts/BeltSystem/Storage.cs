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
    [SerializeField]
    private BeltItemData _beltItem = new BeltItemData();
    private GameObject _newBeltItem = null;

    protected override int _Start()
    {
        return _storageID++;
    }
    
    public override bool CanGiveItem(BeltItem beltItem)
    {
        // empty belt item has id -1, it's nonexistent
        if (this._beltItem.ID == -1)
            return false;
        return this._beltItem.ID == beltItem.ID && this.stackSize > 0;
    }

    public override bool CanPlaceItem(BeltItem beltItem)
    {
        // empty belt item has id -1, it's nonexistent
        if (this._beltItem.ID == -1)
            return true;
        return this._beltItem.ID == beltItem.ID && this.stackSize < 100;
    }

    public override bool CanTakeItem(BeltItem beltItem)
    {
        return CanPlaceItem(beltItem);
    }

    public override void FreeSpace()
    {
        // resets gameobject and lowers space available
        _newBeltItem = null;
        this.stackSize--;
    }

    public override BeltItem GetItem()
    {
        // if storage is empty
        if (this.stackSize == 0)
            return null;
        // if new gameobject is nonexistent
        if(_newBeltItem == null)
        {
            _newBeltItem = Instantiate(this._beltItem.Prefab);
            _newBeltItem.transform.position = GetItemPosition();
            // attaches new beltitem component
            if(_newBeltItem.GetComponent<BeltItem>() == null)
                _newBeltItem.AddComponent<BeltItem>();
            // sets up belt item with correct name and values
            _newBeltItem.GetComponent<BeltItem>().SetupItem(this._beltItem.ID, _newBeltItem);
        }

        return _newBeltItem.GetComponent<BeltItem>();
    }

    public override void ReceiveItem(BeltItem beltItem)
    {
        // increments stack size and destroys previous game object
        if (this._beltItem.ID == -1)
            this._beltItem = beltItemManager.database.beltItemData[beltItem.ID];
        Destroy(beltItem.gameObject);
        this.stackSize++;
    }

}
