using System;
using UnityEngine;

public class OnDestroyCallbackComponent : MonoBehaviour
{
    private Action _onDestroy;

    private void OnDestroy()
    {
        _onDestroy?.Invoke();
    }

    public void AddOnDestroy(Action action)
    {
        _onDestroy += action;
    }
}
