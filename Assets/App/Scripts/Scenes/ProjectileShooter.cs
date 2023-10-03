using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] 
    private SpawnAreasSetuper _spawnAreasSetuper;
    [SerializeField] 
    private ProjectileFactory _projectileFactory;

    private CameraProvider _cameraProvider;
    private float timer = 0;

    public void Construct(CameraProvider cameraProvider)
    {
        _cameraProvider = cameraProvider;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnAreaData areaData = _spawnAreasSetuper.SpawnAreaDatas[0];
            Vector2 worldPosition = _cameraProvider.ViewportToWorldPosition(areaData.ViewportPositionX,
                areaData.ViewportPositionY);
            GameObject fruit = _projectileFactory.CreateFruit(worldPosition);
            Vector2 moveVector = new Vector2(1,1);
            moveVector.x *= Mathf.Cos(Mathf.Deg2Rad * (areaData.LineAngle + areaData.ShootMinAngle));
            moveVector.y *= Mathf.Sin(Mathf.Deg2Rad * (areaData.LineAngle + areaData.ShootMinAngle));
            
            fruit.GetComponent<ShootApplier>().Shoot(moveVector);
            timer = Random.Range(3f, 8f);
        }
    }
}
