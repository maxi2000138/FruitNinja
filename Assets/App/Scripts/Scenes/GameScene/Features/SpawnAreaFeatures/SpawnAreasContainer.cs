using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnAreasContainer : MonoBehaviour
{
    [SerializeField] private ScreenSettingsProvider _screenSettingsProvider;
    [SerializeField] private List<SpawnAreaHandler> _spawnAreaHandlers;

    public List<SpawnAreaData> SpawnAreaHandlers =>
        _spawnAreaHandlers.Select(drawer => drawer.SpawnAreaData).ToList();

    private void OnValidate()
    {
        foreach (SpawnAreaHandler drawer in _spawnAreaHandlers)
        {
            drawer.Construct(_screenSettingsProvider);
            drawer.Validate();
        }
    }

    private void OnDrawGizmos()
    {
        foreach (SpawnAreaHandler drawer in _spawnAreaHandlers)
        {
            drawer.DrawGizmos();
        }
    }
}
