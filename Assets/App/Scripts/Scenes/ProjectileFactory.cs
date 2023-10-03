using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    [SerializeField] private GameObject _fruit;
    
    public GameObject CreateFruit(Vector3 position)
    {
        GameObject fruit = GameObject.Instantiate(_fruit);
        fruit.transform.position = position;
        return fruit;
    }
}
