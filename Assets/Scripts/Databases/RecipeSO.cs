using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RecipeSO : ScriptableObject
{
    public List<RecipeItem> inputItemList;
    public List<RecipeItem> outputItemList;
    public float craftingSpeed;


    public struct RecipeItem
    {
        public BeltItem item;
        public int amount;
    }
}
