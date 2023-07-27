using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RecipeSO : ScriptableObject
{
    public float craftingSpeed;
}

[Serializable]
public class RecipeItem
{
    [field: SerializeField]
    public BeltItemDataSO item;
    [field: SerializeField]
    public int amount;
}