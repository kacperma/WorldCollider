using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new();
    /// <summary>
    /// Places given prefab on position with rotation, returns ID of placed object
    /// </summary>
    internal int PlaceObject(GameObject prefab, Vector3 position, Vector3 rotation, Vector2 objectSize)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position + new Vector3(
            objectSize.x / 2,
            0,
            objectSize.y / 2); 
        newObject.transform.eulerAngles = rotation;
        placedGameObjects.Add(newObject);
        return placedGameObjects.Count - 1;
    }
    /// <summary>
    /// Destroys object with given ID
    /// </summary>
    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
            return;
        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;
    }
}
