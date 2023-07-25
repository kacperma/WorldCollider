using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class Belt : Logistic
{
    private static int _beltID = 0;

    public Belt beltInSequence;
    public Inserter connectedInserter = null;

    protected override int _Start()
    {
        beltInSequence = null;
        beltInSequence = FindNextBelt();
        return _beltID++;
    }

    // change update to run after placing new belt
    public void Update()
    {
        if (beltInSequence == null)
            beltInSequence = FindNextBelt();
        if ((beltItem != null && connectedInserter == null) 
            || (connectedInserter != null && connectedInserter.isSpaceTaken == true))
        {
            StartCoroutine(StartBeltMove());
        }
    }
    /// <summary>
    /// Moves item on belt to next in sequence
    /// </summary>
    private IEnumerator StartBeltMove()
    {
        if (beltItem != null
            && beltInSequence != null
            && beltInSequence.isSpaceTaken == false)
        {
            isSpaceTaken = true;
            Vector3 toPosition = beltInSequence.GetItemPosition();

            beltInSequence.isSpaceTaken = true;
            BeltItem movingBeltItem = beltItem;
            beltItem = null;
            var step = logisticManager.beltSpeed * Time.deltaTime;
            while (movingBeltItem.transform.position != toPosition) 
            {
                movingBeltItem.transform.position = Vector3.MoveTowards(
                    movingBeltItem.transform.position,
                    toPosition,
                    step);
                yield return null;
            }
            isSpaceTaken = false;
            beltInSequence.beltItem = movingBeltItem;
        }
    }
    /// <summary>
    /// Finds new belt in sequence using raycast
    /// </summary>
    private Belt FindNextBelt()
    {
        Transform currentBeltTransform = transform.GetChild(0).transform;
        // offset by 0.2 to hit box collider of next belt
        Vector3 currentBeltPosition = new Vector3(
            currentBeltTransform.position.x,
            currentBeltTransform.position.y + 0.2f,
            currentBeltTransform.position.z);
        RaycastHit hit;

        var forward = currentBeltTransform.forward;
        Ray ray = new Ray(currentBeltPosition, forward);

        //Debug.DrawRay(currentBeltPosition, forward);

        if (Physics.Raycast(ray, out hit, 1f))
        {
            Belt belt = hit.collider.GetComponent<Belt>();
            if (belt != null)
                return belt;
        }
        return null;
    }

    public override bool CanGiveItem(BeltItem beltItem)
    {
        return this.beltItem != null;
    }

    public override bool CanReceiveItem(BeltItem beltItem)
    {
        return true;
    }

    public override BeltItem GetItem()
    {
        return this.beltItem;
    }


    public override void ReceiveItem(BeltItem beltItem)
    {
        this.beltItem = beltItem;
        this.isSpaceTaken = true;
    }

    public override void FreeSpace()
    {
        this.beltItem = null;
        this.isSpaceTaken = false;
    }

    public override bool CanPlaceItem(BeltItem beltItem)
    {
        return this.beltItem == null && this.isSpaceTaken == false; 
    }
}
