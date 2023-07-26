using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    protected Vector2 size;
    protected LogisticManager logisticManager;
    protected const int speedCap = 50;

    protected virtual void Start()
    {
        logisticManager = FindObjectOfType<LogisticManager>();
        int id = _Start();
        gameObject.name = $"{this.GetType().Name}: {id}";
    }
    protected virtual int _Start()
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// Returns object position with Y offset
    /// </summary>
    public Vector3 GetItemPosition()
    {
        float padding = 0.5f;
        Vector3 position = transform.position;
        return new Vector3(position.x, position.y + padding, position.z);
    }

    /// <summary>
    /// Check if object can give item
    /// </summary>
    public virtual bool CanGiveItem(BeltItemDataSO beltItemData)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// Check if object can receive item
    /// </summary>
    public virtual bool CanReceiveItem(BeltItemDataSO beltItemData)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns item data
    /// </summary>
    public virtual BeltItemDataSO GetItemData()
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// Returns reference to item object
    /// </summary>
    public virtual GameObject GetItemObject()
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// Receives given item
    /// </summary>
    public virtual void ReceiveItem(BeltItemDataSO beltItemData, GameObject beltItemObject)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// Frees space, removes reference to item, etc.
    /// </summary>
    public virtual void FreeSpace()
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// Waits until there is space available for item
    /// </summary>
    public virtual bool CanPlaceItem(BeltItemDataSO beltItemData)
    {
        throw new NotImplementedException();
    }
}
