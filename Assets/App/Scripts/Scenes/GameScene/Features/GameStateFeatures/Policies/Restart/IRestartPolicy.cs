using System;

public interface IRestartPolicy
{
    event Action NeedRestart;
}