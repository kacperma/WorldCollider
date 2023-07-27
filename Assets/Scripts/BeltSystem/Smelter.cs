using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter : CraftingMachine
{
    private const int maxStorage = 10;
    private static int _smelterID = 0;
    private int inputStorage = 0;
    private int outputStorage = 0;
    [SerializeField]
    private SmelterRecipeSO selectedRecipe;

    protected override int _Start()
    {
        return _smelterID++;
    }

    public void FixedUpdate()
    {
        if (selectedRecipe != null)
            StartCoroutine(StartSmelter());
    }

    private IEnumerator StartSmelter()
    {
        if (inputStorage >= selectedRecipe.inputItem.amount && outputStorage < maxStorage)
        {
            float craftingSpeed = selectedRecipe.craftingSpeed / logisticManager.smelterSpeed;
            inputStorage -= selectedRecipe.inputItem.amount;
            yield return new WaitForSeconds(craftingSpeed);
            outputStorage++;
        }
    }

    public override bool CanGiveItem(BeltItemDataSO beltItemData)
    {
        if (outputStorage > 0 && selectedRecipe.outputItem.item == beltItemData)
            return true;
        return false;
    }

    public override bool CanPlaceItem(BeltItemDataSO beltItemData)
    {
        if (inputStorage < maxStorage && selectedRecipe.inputItem.item == beltItemData)
            return true;
        return false;
    }

    public override bool CanReceiveItem(BeltItemDataSO beltItemData)
    {
        return CanPlaceItem(beltItemData);
    }

    public override void FreeSpace()
    {
        this.outputStorage--;
    }

    public override void ReceiveItem(BeltItemDataSO beltItemData, GameObject beltItemObject)
    {
        inputStorage++;
        Destroy(beltItemObject);
    }
    public override BeltItemDataSO GetItemData()
    {
        return this.selectedRecipe.outputItem.item;
    }

    public override GameObject GetItemObject()
    {
        GameObject returnObject = Instantiate(this.selectedRecipe.outputItem.item.Prefab, this.GetItemPosition(), Quaternion.identity);
        returnObject.name = this.selectedRecipe.outputItem.item.Name;
        return returnObject;
    }
}
