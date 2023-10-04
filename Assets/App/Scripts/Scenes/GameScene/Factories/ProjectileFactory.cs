using UnityEngine;

public class ProjectileFactory : IProjectileFactory
{
    private readonly IDestroyLine _destroyLine;
    private readonly ProjectileContainer _projectileContainer;

    public ProjectileFactory(IDestroyLine destroyLine, ProjectileContainer projectileContainer)
    {
        _destroyLine = destroyLine;
        _projectileContainer = projectileContainer;
    }

    public GameObject CreateDemoFruit(Vector2 position)
    {
        GameObject fruit = GameObject.Instantiate((GameObject)Resources.Load(ResourcePathes.BaseFruitPath), _projectileContainer.transform, true);
        fruit.transform.position = position;
        _destroyLine.AddLineDestroyListener(fruit.transform);
        return fruit;
    }
}
