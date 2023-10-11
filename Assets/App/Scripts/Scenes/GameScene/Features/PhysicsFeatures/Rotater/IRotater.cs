using UnityEngine;

public interface IRotater
{
    public Vector3 Rotate(float deltaTime);
    public void Clear();
}