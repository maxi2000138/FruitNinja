using System.Collections.Generic;
using UnityEngine;

public class SpawnAreasSetuper : MonoBehaviour
{
    [SerializeField] private List<SpawnAreaData> _spawnAreaDatas;

    public IReadOnlyList<SpawnAreaData> SpawnAreaDatas =>
        _spawnAreaDatas;

    private void OnValidate()
    {
        foreach (SpawnAreaData data in _spawnAreaDatas)
        {
            data.Validate();
        }
    }

    private void OnDrawGizmos()
    {
        foreach (SpawnAreaData data in _spawnAreaDatas)
        {
            data.DrawGizmos();
        }
    }
}
