using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnAreasSetuper : MonoBehaviour
{
    [SerializeField] private List<SpawnAreaDrawer> _spawnAreaDrawers;

    public List<SpawnAreaData> SpawnAreaDatas =>
        _spawnAreaDrawers.Select(drawer => drawer.SpawnAreaData).ToList();

    private void OnValidate()
    {
        foreach (SpawnAreaDrawer drawer in _spawnAreaDrawers)
        {
            drawer.Validate();
        }
    }

    private void OnDrawGizmos()
    {
        foreach (SpawnAreaDrawer drawer in _spawnAreaDrawers)
        {
            drawer.DrawGizmos();
        }
    }
}
