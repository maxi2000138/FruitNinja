using UnityEngine;

public class ProjectileFactory
{
    public GameObject CreateFruit(Vector2 position)
    {
        GameObject fruit = GameObject.Instantiate((GameObject)Resources.Load(ResourcePathes.BaseFruitPath));
        fruit.transform.position = position;
        return fruit;
    }
}
