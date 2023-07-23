using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logistic : Structure
{
    public BeltItem beltItem;
    public bool isSpaceTaken;

    private void Start()
    {
        logisticManager = FindObjectOfType<LogisticManager>();
        int id = _Start();
        gameObject.name = $"{this.GetType().Name}: {id}";
    }

    /// <summary>
    /// Virtual method to initialize all classes that are childre
    /// </summary>
    protected virtual int _Start()
    {
        throw new NotImplementedException();
    }
    private void OnDestroy()
    {
        if (beltItem != null)
            Destroy(beltItem.item);
    }
}
