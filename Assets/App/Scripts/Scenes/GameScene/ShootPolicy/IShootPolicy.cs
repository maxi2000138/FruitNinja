using System;

public interface IShootPolicy
{
    public event Action NeedShoot;
    public void StartWorking();
    public void StopWorking();
}
