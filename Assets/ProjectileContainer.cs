using System.Collections.Generic;

public class ProjectileContainer
{
    public Dictionary<ProjectileType, List<ProjectileObject>> _activeProjectiles { get; private set; } = new();

    public void AddToDictionary(ProjectileType type, ProjectileObject projectileObject)
    {
        if(_activeProjectiles.ContainsKey(type))
            _activeProjectiles[type].Add(projectileObject);
        else
            _activeProjectiles.Add(type, new List<ProjectileObject>() { projectileObject });
    }

    public void RemoveFromDictionary(ProjectileObject projectileObject)
    {
        foreach (List<ProjectileObject> projectileObjects in _activeProjectiles.Values)
        {
            if (projectileObjects.Contains(projectileObject))
                projectileObjects.Remove(projectileObject);
        }
    }
}
