using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logistic : Structure
{
    public BeltItemDataSO beltItemData;
    public bool isSpaceTaken;
    public GameObject beltItemObject = null;

    /// <summary>
    /// Virtual method to initialize all classes that are childre
    /// </summary>
    protected override int _Start()
    {
        throw new NotImplementedException();
    }

    public override BeltItemDataSO GetItemData()
    {
        return this.beltItemData;
    }

    public override GameObject GetItemObject()
    {
        return this.beltItemObject;
    }

    private void OnDestroy()
    {
        Destroy(this.beltItemObject);
    }
}
