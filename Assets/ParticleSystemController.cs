using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _particleSystems;

    public void SetColor(Color color)
    {
        for (int i = 0; i < _particleSystems.Count; i++)
        {
            ParticleSystem.MainModule mainModule = _particleSystems[i].main;
            mainModule.startColor = color;
        }
    }

    public void PlayAll()
    {
        for (int i = 0; i < _particleSystems.Count; i++)
        {
            _particleSystems[i].Play();
        }
    }
}
