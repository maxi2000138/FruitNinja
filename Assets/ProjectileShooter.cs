using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private SpawnAreasSetuper _spawnAreasSetuper;
    [SerializeField] private ProjectileFactory _projectileFactory;

    private float timer = 0;
    
    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnAreaData areaData = _spawnAreasSetuper.SpawnAreaDatas[0];
            GameObject fruit = _projectileFactory.CreateFruit(areaData.Point);
            Vector3 moveVector = Vector3.up;
            
            fruit.GetComponent<ShootApplier>().Shoot(areaData.MinAnglePoint.normalized);
            timer = Random.Range(3f, 8f);
        }
    }
}
