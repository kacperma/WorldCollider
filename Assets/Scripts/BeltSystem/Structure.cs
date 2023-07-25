using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    protected Vector2 size;
    protected LogisticManager logisticManager;

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
    public virtual bool CanGiveItem(BeltItem beltItem)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// Check if object can receive item
    /// </summary>
    public virtual bool CanReceiveItem(BeltItem beltItem)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// Returns reference to item
    /// </summary>
    public virtual BeltItem GetItem()
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// Receives given item
    /// </summary>
    public virtual void ReceiveItem(BeltItem beltItem)
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
    public virtual bool CanPlaceItem(BeltItem beltItem)
    {
        throw new NotImplementedException();
    }
}
