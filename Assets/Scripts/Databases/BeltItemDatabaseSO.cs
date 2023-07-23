using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BeltItemDatabaseSO : ScriptableObject
{
    public List<BeltItemData> beltItemData;
}


[Serializable]
public class BeltItemData
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; } = -1;
    [field: SerializeField]
    public GameObject Prefab { get; private set; }

}