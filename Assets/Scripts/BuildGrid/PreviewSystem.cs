using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float previewYOffset = 0.06f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField]
    private Material previewMaterialPrefab;
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }
    /// <summary>
    /// Initiates placement preview of given prefab and its size
    /// </summary>
    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
    }
    /// <summary>
    /// Scales cursor to correct size
    /// </summary>
    private void PrepareCursor(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);
            cellIndicatorRenderer.material.mainTextureScale = size;
        }
    }
    /// <summary>
    /// Prepares preview of given prefab, makes its transparent
    /// </summary>
    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
        // TODO MOVE IT SOMEWHERE THAT MAKES SENSE
        if (previewObject.GetComponent<Belt>() != null)
        {
            previewObject.GetComponent<Belt>().enabled = false;
            previewObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
    /// <summary>
    /// Stops showing preview
    /// </summary>
    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        if(previewObject != null)
            Destroy(previewObject);
    }
    /// <summary>
    /// Updates position of previewObject
    /// </summary>
    public void UpdatePosition(Vector3 position, bool validity)
    {
        if (previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(validity);
        }

        MoveCursor(position);
        ApplyFeedbackToCursor(validity);
    }
    /// <summary>
    /// Applies color to previewObject, white = empty place, red = place occupied
    /// </summary>
    private void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        previewMaterialInstance.color = c;
    }
    /// <summary>
    /// Applies color to cursor, green = empty place, red = place occupied
    /// </summary>
    private void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.green : Color.red;
        cellIndicatorRenderer.material.color = c;
    }
    /// <summary>
    /// Moves cursor to given position
    /// </summary>
    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }
    /// <summary>
    /// Moves previewObject to given position
    /// </summary>
    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x, position.y + previewYOffset, position.z);
    }
    /// <summary>
    /// Initiates remove preview of given prefab and its size
    /// </summary>
    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);
    }
    /// <summary>
    /// Rotates pivot by 90 degrees
    /// </summary>
    internal void Rotate()
    {
        // IMPORTANT
        // every prefab should have only one child (pivot) and it's origin should be on center of rotation
        // if prefab is size 1x1
        // (0.5, 0, 0.5)
        // if prefab is size 3x3
        // (1.5, 0, 1.5)
        // for now different sizes are not supported, because of rotation and snapping to correct grid pos

        Transform prefabChild = previewObject.transform.GetChild(0);
        prefabChild.transform.RotateAround(prefabChild.transform.position, Vector3.up, 90);
    }
    /// <summary>
    /// Gets eulerAngles of first child (pivot)
    /// </summary>
    internal Vector3 GetRotation()
    {
        return previewObject.transform.GetChild(0).transform.eulerAngles;
    }
}
