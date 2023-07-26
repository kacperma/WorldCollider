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

    //change update to run after placing new belt
    public void FixedUpdate()
    {
        if (beltInSequence == null)
            beltInSequence = FindNextBelt();
        if (beltItemData != null)
            StartCoroutine(StartBeltMove());
    }
    /// <summary>
    /// Moves item on belt to next in sequence
    /// </summary>
    private IEnumerator StartBeltMove()
    {
        if (beltItemData != null
            && beltInSequence != null
            && beltInSequence.isSpaceTaken == false)
        {
            isSpaceTaken = true;
            Vector3 toPosition = beltInSequence.GetItemPosition();

            beltInSequence.isSpaceTaken = true;
            BeltItemDataSO movingBeltItemData = beltItemData;
            GameObject movingBeltItemObject = beltItemObject;

            beltItemData = null;
            beltItemObject = null;

            var step = logisticManager.beltSpeed / speedCap;
            while (movingBeltItemObject.transform.position != toPosition) 
            {
                movingBeltItemObject.transform.position = Vector3.MoveTowards(
                    movingBeltItemObject.transform.position,
                    toPosition,
                    step);
                yield return null;
            }
            isSpaceTaken = false;
            beltInSequence.beltItemData = movingBeltItemData;
            beltInSequence.beltItemObject = movingBeltItemObject;
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

    public override bool CanGiveItem(BeltItemDataSO beltItem)
    {
        return this.beltItemData != null;
    }

    public override bool CanReceiveItem(BeltItemDataSO beltItem)
    {
        return true;
    }

    public override void ReceiveItem(BeltItemDataSO beltItemData, GameObject beltItemObject)
    {
        this.beltItemData = beltItemData;
        this.beltItemObject = beltItemObject;
        this.isSpaceTaken = true;
    }

    public override void FreeSpace()
    {
        this.beltItemData = null;
        this.beltItemObject = null;
        this.isSpaceTaken = false;
    }

    public override bool CanPlaceItem(BeltItemDataSO beltItem)
    {
        return this.beltItemData == null && this.isSpaceTaken == false; 
    }
}
