using App.Scripts.Scenes.GameScene.Configs;
using UnityEngine;

public class PauseController
{
    private readonly PhysicsConfig _physicsConfig;

    public PauseController(PhysicsConfig physicsConfig)
    {
        _physicsConfig = physicsConfig;
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
