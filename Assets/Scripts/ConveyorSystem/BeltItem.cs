using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltItem : MonoBehaviour
{
    public GameObject item;
    //introduce item database with ID, similar to build system

    private void Awake()
    {
        item = gameObject;
    }
}
