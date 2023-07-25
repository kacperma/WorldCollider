using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltItem : MonoBehaviour
{
    public GameObject item;
    public int ID = 0;

    /// <summary>
    /// Sets up belt item with correct id
    /// </summary>
    public void SetupItem(int _id, GameObject _item)
    {
        //name = $"{beltItemManager.database.beltItemData[_id].Name}";
        ID = _id;
        item = _item;
    }
}