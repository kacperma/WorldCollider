using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;
    GridData objectData;

    [SerializeField]
    private ObjectsDatabaseSO database;

    [SerializeField]
    private PreviewSystem previewSystem;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        objectData = new();
    }
    /// <summary>
    /// Changes visibility of grid visualizations
    /// </summary>
    private void ChangeGridState(bool state)
    {
        GameObject[] gridsVisualization = GameObject.FindGameObjectsWithTag("GridVisualization");
        foreach (GameObject grid in gridsVisualization)
        {
            grid.GetComponent<MeshRenderer>().enabled = state;
        }
    }

    /// <summary>
    /// Shows grid and initiates object placement state
    /// </summary>
    public void StartPlacement(int ID)
    {
        StopPlacement();
        ChangeGridState(true);
        buildingState = new PlacementState(ID,
                                           grid,
                                           previewSystem,
                                           database,
                                           objectData,
                                           objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }
    /// <summary>
    /// Shows grid and initiates object removal state
    /// </summary>
    public void StartRemove()
    {
        StopPlacement();
        ChangeGridState(true);
        buildingState = new RemoveState(grid, previewSystem, objectData,objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }
    /// <summary>
    /// Places structure on grid position pointed by cursor
    /// </summary>
    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUi())
            return;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        buildingState.OnAction(gridPosition);
    }
    /// <summary>
    /// Hides grid and stops object placement state
    /// </summary>
    private void StopPlacement()
    {
        ChangeGridState(false);
        if (buildingState == null)
            return;
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null)
            return;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if(lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            buildingState.Rotate();
        }
    }
}
