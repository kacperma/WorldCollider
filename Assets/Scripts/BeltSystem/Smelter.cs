using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter : Machine
{
    public override bool CanGiveItem(BeltItem beltItem)
    {
        return base.CanGiveItem(beltItem);
    }

    public override bool CanPlaceItem(BeltItem beltItem)
    {
        return base.CanPlaceItem(beltItem);
    }

    public override bool CanReceiveItem(BeltItem beltItem)
    {
        return base.CanReceiveItem(beltItem);
    }

    public override void FreeSpace()
    {
        base.FreeSpace();
    }

    public override BeltItem GetItem()
    {
        return base.GetItem();
    }

    public override void ReceiveItem(BeltItem beltItem)
    {
        base.ReceiveItem(beltItem);
    }
}
