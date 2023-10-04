using UnityEngine;

public class ProjectileShooter : IUpdatable
{
    private readonly SpawnAreasSetuper _spawnAreasSetuper;
    private readonly CameraFeaturesProvider _cameraFeaturesProvider;
    private readonly ProjectileFactory _projectileFactory;
    private float timer = 0;

    public ProjectileShooter(ProjectileFactory projectileFactory, SpawnAreasSetuper spawnAreasSetuper, CameraFeaturesProvider cameraFeaturesProvider)
    {
        _projectileFactory = projectileFactory;
        _spawnAreasSetuper = spawnAreasSetuper;
        _cameraFeaturesProvider = cameraFeaturesProvider;
    }
    
    public void Update(float deltaTime)
    {
        timer -= deltaTime;

        if (timer <= 0)
        {
            SpawnAreaData areaData = _spawnAreasSetuper.SpawnAreaHandlers.GetRandomItemByProbability(data => data.Probability);
            float angle = (areaData.ShootMinAngle, areaData.ShootMaxAngle).GetRandomFloatBetween();

            Vector2 worldPosition = new Vector2(areaData.ViewportPositionX, areaData.ViewportPositionY);
            Vector2 lineDelta = new Vector2(1, 1);
            Vector2 position = _cameraFeaturesProvider.ViewportToWorldPosition((areaData.ViewportLeftPosition, areaData.ViewportRightPosition).GetRandomPointBetween());
            GameObject fruit = _projectileFactory.CreateFruit(position);
            Vector2 moveVector = new Vector2(1,1);
            moveVector.x *= Mathf.Cos(Mathf.Deg2Rad * (areaData.LineAngle + angle));
            moveVector.y *= Mathf.Sin(Mathf.Deg2Rad * (areaData.LineAngle + angle));
            fruit.GetComponent<ShootApplier>().Shoot(moveVector);
            timer = Random.Range(0.1f, 0.5f);
        }
    }
}
