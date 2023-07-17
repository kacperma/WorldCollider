using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveState : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    PreviewSystem previewSystem;
    GridData objectData;
    ObjectPlacer objectPlacer;

    public RemoveState(Grid grid,
                       PreviewSystem previewSystem,
                       GridData objectData,
                       ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.objectData = objectData;
        this.objectPlacer = objectPlacer;

        previewSystem.StartShowingRemovePreview();
    }
    /// <summary>
    /// Stops showing preview
    /// </summary>
    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    /// <summary>
    /// Removes object on given gridPosition
    /// </summary>
    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        if (objectData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectedData = objectData;
        }

        if(selectedData == null)
        {
            return;
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if (gameObjectIndex == -1)
                return;
            selectedData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }
        Vector3 cellPosition = grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));
    }
    
    /// <summary>
    /// Updates object on given gridPosition
    /// </summary>
    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), !validity);
    }
    /// <summary>
    /// Checks if on given gridPosition is object to remove
    /// </summary>
    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return objectData.CanPlaceObjectAt(gridPosition, Vector2Int.one);
    }

    /// <summary>
    /// Interface compliance, does nothing
    /// </summary>
    public void Rotate()
    {
        return;
    }
}
