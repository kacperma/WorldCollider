using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridData objectData;
    ObjectPlacer objectPlacer;

    public PlacementState(int iD,
        Grid grid,
        PreviewSystem previewSystem,
        ObjectsDatabaseSO database,
        GridData objectData,
        ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.objectData = objectData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab, database.objectsData[selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"No object with ID{iD}");
        }
    }

    /// <summary>
    /// Stops showing preview
    /// </summary>
    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    /// <summary>
    /// Places object on given gridPosition if it's not occupied and adds it to objectData list
    /// </summary>
    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
            return;
        GameObject prefab = database.objectsData[selectedObjectIndex].Prefab;
        Vector3 rotation = previewSystem.GetRotation();
        int index = objectPlacer.PlaceObject(prefab, grid.CellToWorld(gridPosition), rotation);

        objectData.AddObjectAt(
            gridPosition,
            database.objectsData[selectedObjectIndex].Size,
            database.objectsData[selectedObjectIndex].ID,
            index);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }
    /// <summary>
    /// Updates object on given gridPosition
    /// </summary>
    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
    /// <summary>
    /// Checks if given gridPosition is valid for placement
    /// </summary>
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        return objectData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }
    /// <summary>
    /// Rotates preview by 90 degrees
    /// </summary>
    public void Rotate()
    {
        previewSystem.Rotate();
    }

}
