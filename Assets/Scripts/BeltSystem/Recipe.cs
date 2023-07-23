using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{
    public Dictionary<BeltItem, int> inputItems = new();
    public BeltItem outputItem;
    public float craftSpeed;
}
