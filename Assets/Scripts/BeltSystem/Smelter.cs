using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter : Machine
{
    private const int maxStorage = 10;
    private static int _smelterID = 0;
    private int inputStorage = 0;
    private int outputStorage = 0;

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
        if (inputStorage >= selectedRecipe.inputItemList[0].amount && outputStorage < maxStorage)
        {
            float craftingSpeed = selectedRecipe.craftingSpeed / logisticManager.smelterSpeed;
            inputStorage -= selectedRecipe.inputItemList[0].amount;
            yield return new WaitForSeconds(craftingSpeed);
            outputStorage++;
        }
    }

    public override bool CanGiveItem(BeltItemDataSO beltItemData)
    {
        if (outputStorage > 0 && selectedRecipe.outputItemList[0].item == beltItemData)
            return true;
        return false;
    }

    public override bool CanPlaceItem(BeltItemDataSO beltItemData)
    {
        if (inputStorage < maxStorage && selectedRecipe.inputItemList[0].item == beltItemData)
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
        return this.selectedRecipe.outputItemList[0].item;
    }

    public override GameObject GetItemObject()
    {
        GameObject returnObject = Instantiate(this.selectedRecipe.outputItemList[0].item.Prefab, this.GetItemPosition(), Quaternion.identity);
        returnObject.name = this.selectedRecipe.outputItemList[0].item.Name;
        return returnObject;
    }
}
