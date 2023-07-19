using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PreviewSystem : MonoBehaviour
{
	[SerializeField]
	private float previewYOffset = 0.6f;

	[SerializeField]
	private GameObject cellIndicator;
	private GameObject previewObject;

	[SerializeField]
	private Material previewMaterialPrefab;
	private Material previewMaterialInstance;

	private Renderer cellIndicatorRenderer;

	private Vector3 gridOffset;
	private void Start()
	{
		previewMaterialInstance = new Material(previewMaterialPrefab);
		cellIndicator.SetActive(false);
		cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
	}
	/// <summary>
	/// Initiates placement preview of given prefab and its size
	/// </summary>
	public void StartShowingPlacementPreview(GameObject prefab, Vector2 size)
	{
		gridOffset = new Vector3(
			size.x / 2,
			0,
			size.y / 2);
		previewObject = Instantiate(prefab);
		PreparePreview(previewObject);
		PrepareCursor(size);
		cellIndicator.SetActive(true);
	}
	/// <summary>
	/// Scales cursor to correct size
	/// </summary>
	private void PrepareCursor(Vector2 size)
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
		// find some simpler way to disable all scripts on this object
		if (previewObject.GetComponentsInChildren<MonoBehaviour>() != null)
		{
			foreach (var comp in previewObject.GetComponentsInChildren<MonoBehaviour>())
			{
				comp.enabled = false;
			}
		}
		if (previewObject.GetComponentInChildren<BoxCollider>() != null)
			previewObject.GetComponentInChildren<BoxCollider>().enabled = false;
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
		cellIndicator.transform.position = new Vector3(
			position.x,
			position.y + previewYOffset,
			position.z) + gridOffset;
	}
	/// <summary>
	/// Moves previewObject to given position
	/// </summary>
	private void MovePreview(Vector3 position)
	{
		previewObject.transform.position = position + gridOffset;
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
		previewObject.transform.RotateAround(previewObject.transform.position, Vector3.up, 90);
	}
	/// <summary>
	/// Gets eulerAngles of first child (pivot)
	/// </summary>
	internal Vector3 GetRotation()
	{
		return previewObject.transform.eulerAngles;
	}
}
