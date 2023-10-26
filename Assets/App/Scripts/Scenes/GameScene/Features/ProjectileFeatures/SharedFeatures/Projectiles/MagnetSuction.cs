using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MagnetSuction : MonoBehaviour, ILooseGameListener
{
    private bool _isActive;
    private readonly List<Vector2> _positions = new();
    private ProjectileContainer _projectileContainer;
    private BonusesConfig _bonusesConfig;
    private List<ProjectileObject> _activeProjectile;
    private readonly List<ProjectileObject> _suctionProjectile = new();
    private readonly List<MagnetSuctionApplier> _suctionMagnet = new();
    private float _distance;
    private float _force;
    private bool _setupedApplier;
    private TokenController _tokenController;


    public void Construct(ProjectileContainer projectileContainer, BonusesConfig bonusesConfig)
    {
        _tokenController = new TokenController();
        _projectileContainer = projectileContainer;
        _bonusesConfig = bonusesConfig;
        _isActive = false;
    }

    private void Update()
    {
        if(!_isActive)
            return;

        if(_projectileContainer._activeProjectiles.TryGetValue(ProjectileType.Fruit, out var projectile) && projectile != null)
            _activeProjectile = projectile;
        
        for (int i = 0; i < _activeProjectile.Count; i++)
        {
            IterateAllSuctionPositionsAndAddSuctionIfInRange(i);
            ClearSuctionIfOutOfRange(i);
        }
        
    }

    private void ClearSuctionIfOutOfRange(int i)
    {
        if (!_setupedApplier && _suctionProjectile.Contains(_activeProjectile[i]))
        {
            int indexOf = _suctionProjectile.IndexOf(_activeProjectile[i]);
            ClearSuctionApplier(i, indexOf);
        }
    }

    private void IterateAllSuctionPositionsAndAddSuctionIfInRange(int i)
    {
        _setupedApplier = false;

        for (int j = 0; j < _positions.Count; j++)
        {
            _distance = Vector2.Distance(_activeProjectile[i].transform.position, _positions[j]);
            if (_distance > _bonusesConfig.FarthestMagnetDistance)
                continue;

            if (!_suctionProjectile.Contains(_activeProjectile[i]))
            {
                CreateSuctionApplier(i, j);
                break;
            }
        }
    }

    private void CreateSuctionApplier(int i, int j)
    {
        MagnetSuctionApplier suctionApplier = new MagnetSuctionApplier(_activeProjectile[i], _positions[j], _bonusesConfig);
        _suctionProjectile.Add(_activeProjectile[i]);
        _activeProjectile[i].GravitationApplier.Disable();
        _suctionMagnet.Add(suctionApplier);
        _activeProjectile[i].Mover.AddMover(suctionApplier);
        _setupedApplier = true;
    }

    private void ClearSuctionApplier(int i, int index)
    {
        _activeProjectile[i].Mover.RemoveMover(_suctionMagnet[index]);
        _suctionProjectile.RemoveAt(index);
        _suctionMagnet.RemoveAt(index);
        _activeProjectile[i].GravitationApplier.Enable();
    }
    
    public async void StartSuction(Vector2 position, float activeSecondsTime)
    {
        _positions.Add(position);
        _isActive = true;
        await UniTask.Delay((int)(activeSecondsTime*1000), DelayType.DeltaTime
            , PlayerLoopTiming.Update, _tokenController.CreateCancellationToken());
        _positions.Remove(position);
        TryStopSuction();
    }

    private void TryStopSuction()
    {
        if (_positions.Count == 0)
        {
            _isActive = false;

            for (int i = 0; i < _activeProjectile.Count; i++)
            {
                int indexOf = _suctionProjectile.IndexOf(_activeProjectile[i]);
                if(indexOf != -1)
                    ClearSuctionApplier(i, indexOf);
            }
        }
    }

    public void OnLooseGame()
    {
        _tokenController.CancelTokens();
    }
}
