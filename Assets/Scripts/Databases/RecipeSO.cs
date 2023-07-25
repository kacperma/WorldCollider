using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RecipeSO : ScriptableObject
{
    public List<RecipeItem> inputItemList;
    public List<RecipeItem> outputItemList;
    public float craftingSpeed;

    [Serializable]
    public struct RecipeItem
    {
        public BeltItemDataSO item;
        public int amount;
    }
}
