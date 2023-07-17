using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Belt : MonoBehaviour
{
    private static int _beltID = 0;

    public Belt beltInSequence;
    public BeltItem beltItem;
    public bool isSpaceTaken;

    private BeltManager beltManager;

    private void Start()
    {
        beltManager = FindObjectOfType<BeltManager>();
        beltInSequence = null;
        beltInSequence = FindNextBelt();
        gameObject.name = $"Belt: {_beltID++}";
    }
    // change update to run after placing new belt
    private void Update()
    {
        if (beltInSequence == null)
            beltInSequence = FindNextBelt();
        if (beltItem != null && beltItem.item != null)
            StartCoroutine(StartBeltMove());
    }
    /// <summary>
    /// Get current position of item on belt
    /// </summary>
    public Vector3 GetItemPosition()
    {
        float padding = 0.5f;
        Vector3 position = transform.GetChild(0).transform.position;
        return new Vector3(position.x, position.y + padding, position.z);
    }
    /// <summary>
    /// Moves item on belt to next in sequence
    /// </summary>
    private IEnumerator StartBeltMove()
    {
        isSpaceTaken = true;
        if (beltItem.item != null && beltInSequence != null && beltInSequence.isSpaceTaken == false)
        {
            Vector3 toPosition = beltInSequence.GetItemPosition();

            beltInSequence.isSpaceTaken = true;

            var step = beltManager.speed * Time.deltaTime;
            while(beltItem.item.transform.position != toPosition) 
            {
                beltItem.item.transform.position = Vector3.MoveTowards(
                    beltItem.transform.position,
                    toPosition,
                    step);
                yield return null;
            }

            isSpaceTaken = false;
            beltInSequence.beltItem = beltItem;
            beltItem = null;
        }
    }
    /// <summary>
    /// Finds new belt in sequence using raycast
    /// </summary>
    private Belt FindNextBelt()
    {
        Transform currentBeltTransform = transform.GetChild(0).transform;
        Vector3 currentBeltPosition = new Vector3(
            currentBeltTransform.position.x,
            currentBeltTransform.position.y + 0.2f,
            currentBeltTransform.position.z);
        RaycastHit hit;

        var right = currentBeltTransform.right;
        Ray ray = new Ray(currentBeltPosition, right);

        Debug.DrawRay(currentBeltPosition, right);

        if (Physics.Raycast(ray, out hit, 1f))
        {
            Debug.Log(hit.collider);
            Belt belt = hit.collider.GetComponent<Belt>();
            if (belt != null)
                return belt;
        }
        return null;
    }

}
