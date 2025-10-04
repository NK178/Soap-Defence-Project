using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "SpawnerData", menuName = "Scriptable Objects/SpawnerData")]
public class SpawnerData : ScriptableObject
{
    public bool fastSpawn = false;
    public List<SpawnDetails> dataList;
}





