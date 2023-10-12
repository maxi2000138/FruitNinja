using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ResourceFeatures
{
    public class ResourceObjectsProvider
    {
        private readonly Dictionary<string, GameObject> _objectsByPath = new();

        public GameObject GetGameObject(string resourcePath)
        {
            GameObject loadGameObject;
            if (_objectsByPath.TryGetValue(resourcePath, out loadGameObject))
                return loadGameObject;
        
            loadGameObject = LoadAndCashGameObject(resourcePath);
            return loadGameObject;
        }

        private GameObject LoadAndCashGameObject(string resourcePath)
        {
            GameObject loadGameObject;
            loadGameObject = (GameObject)Resources.Load(resourcePath);
            _objectsByPath.Add(resourcePath, loadGameObject);
            return loadGameObject;
        }
    }
}
