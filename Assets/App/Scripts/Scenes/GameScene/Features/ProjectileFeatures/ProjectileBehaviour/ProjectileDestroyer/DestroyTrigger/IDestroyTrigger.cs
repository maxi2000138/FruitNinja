using UnityEngine;

public interface IDestroyTrigger
{
    void AddDestroyTriggerListeners(params Transform[] objectTransform);
}