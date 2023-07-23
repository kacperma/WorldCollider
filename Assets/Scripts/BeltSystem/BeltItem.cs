using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltItem : MonoBehaviour
{
    public GameObject item;
    public int ID = 0;

    public BeltItem(int iD)
    {
        ID = iD;
    }

    //introduce item database with ID, similar to build system

    private void Awake()
    {
        item = gameObject;
    }
}
