using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inserter : Logistic
{
    private static int _inserterID = 0;
    [SerializeField]
    protected Structure startPoint;
    [SerializeField]
    protected Structure endPoint;
    [SerializeField]
    protected GameObject inserterHead = null;


    protected override int _Start()
    {
        startPoint = null;
        endPoint = null;
        FindEndPoints();
        return _inserterID++;
    }

    public void Update()
    {
        if (startPoint == null || endPoint == null)
        {
            FindEndPoints();
        }
        if (beltItem == null
            && startPoint != null
            && endPoint != null
            && isSpaceTaken == false
            && startPoint.GetItem() != null)
            StartCoroutine(StartInserterMove());
    }

    /// <summary>
    /// Starts inserter movement
    /// </summary>
    public IEnumerator StartInserterMove()
    {
        if (startPoint.GetItem() != null
            && endPoint != null && isSpaceTaken == false)
        {
            isSpaceTaken = true;
            Vector3 startPosition = inserterHead.transform.position;
            Vector3 toPosition = endPoint.GetItemPosition();

            var step = logisticManager.inserterSpeed * Time.deltaTime;

            if (endPoint.GetComponent<MonoBehaviour>() is Structure && startPoint.GetComponent<MonoBehaviour>() is Structure)
            {
                Structure logisticEndPoint = (Structure)endPoint.GetComponent<MonoBehaviour>();
                Structure logisticStartPoint = (Structure)startPoint.GetComponent<MonoBehaviour>();
                beltItem = logisticStartPoint.GetItem();
                bool canReceive = logisticEndPoint.CanReceiveItem(beltItem);
                bool canGive = logisticStartPoint.CanGiveItem(beltItem);
                if (canReceive && canGive)
                {
                    logisticStartPoint.FreeSpace();
                    // moves item and inserter head to end point position
                    while (beltItem.transform.position != toPosition)
                    {
                        beltItem.transform.position = Vector3.MoveTowards(
                            beltItem.transform.position,
                            toPosition,
                            step / 2f);
                        inserterHead.transform.position = Vector3.MoveTowards(
                            inserterHead.transform.position,
                            new Vector3(toPosition.x, startPosition.y, toPosition.z),
                            step / 2f);
                        yield return null;
                    }
                    // waits until space is available
                    while (!logisticEndPoint.CanPlaceItem(beltItem))
                    {
                        yield return null;
                    }
                    // endpoint receives item
                    logisticEndPoint.ReceiveItem(beltItem);
                    this.beltItem = null;
                    // moves inserter head back to start position
                    while (inserterHead.transform.position != startPosition)
                    {
                        inserterHead.transform.position = Vector3.MoveTowards(
                            inserterHead.transform.position,
                            startPosition,
                            step / 2f);
                        yield return null;
                    }
                    // frees inserter space 
                    isSpaceTaken = false;
                }
            }
        }
    }
    /// <summary>
    /// Finds end point and start point
    /// </summary>
    public virtual void FindEndPoints()
    {
        Transform currentInserterHeadTransform = inserterHead.transform;

        Vector3 currentInserterPosition = currentInserterHeadTransform.position;
        RaycastHit hit;
        var down = -currentInserterHeadTransform.up;

        //shots ray down from inserter head to find start point
        Ray ray = new Ray(currentInserterPosition, down);
        //Debug.DrawRay(currentInserterPosition, down, Color.red);
        if (Physics.Raycast(ray, out hit, 2f))
        {
            Structure logisticStructure = (Structure)hit.collider.GetComponent<MonoBehaviour>();
            if (logisticStructure != null)
            {
                if (logisticStructure is Belt)
                {
                    Belt belt = (Belt)logisticStructure;
                    belt.connectedInserter = this;
                }
                startPoint = logisticStructure;
            }

        }
        // new ray position oposide of inserter head
        currentInserterPosition = currentInserterHeadTransform.TransformPoint(Vector3.forward
            * 2 * Math.Abs(currentInserterHeadTransform.localPosition.z));
        //shots ray down from inserter head to find end point
        ray = new Ray(currentInserterPosition, down);
        //Debug.DrawRay(currentInserterPosition, down, Color.green);
        if (Physics.Raycast(ray, out hit, 2f))
        {
            Structure logisticStructure = (Structure)hit.collider.GetComponent<MonoBehaviour>();
            if (logisticStructure != null)
            {
                endPoint = logisticStructure;
            }
        }
    }
}
