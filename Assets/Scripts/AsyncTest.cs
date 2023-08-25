using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class AsyncTest : MonoBehaviour
{
    private async void Start()
    {
        Debug.Log("Start");
        NoNameToken token = new NoNameToken(this);
        _ = TestAsync(token);

        Destroy(this.gameObject);
        //Pauseé¿å±

        //token.Pause();
        //await Awaitable.WaitForSecondsAsync(2.0f);
        //token.Restart();



        //Cancelé¿å±
        //token.Cancel();


        Debug.Log("End");
    }

    private async Awaitable TestAsync(NoNameToken cancellationToken)
    {
        Debug.Log("TestAsyncStart");
        await NoNameAwaitable.WaitForSecondsAsync(1.0f, cancellationToken);
        Debug.Log("TestAsyncEnd");
    }
}
public class NoNameToken
{
    public bool isPaused { get; private set; } = false;
    public CancellationToken cancellationToken { get; private set; }

    private CancellationTokenSource _cancellationSource = null;
    private MonoBehaviour _monoBehaviour;

    public NoNameToken(MonoBehaviour mono)
    {
        this._monoBehaviour = mono;

        if (_cancellationSource == null)
        {
            _cancellationSource = new CancellationTokenSource();
        }
        cancellationToken = _cancellationSource.Token;

        var onDestroyC = _monoBehaviour.AddComponent<OnDestroyCallbackComponent>();
        onDestroyC.AddOnDestroy(() => _cancellationSource.Cancel());
    }

    public void Pause()
    {
        isPaused = true;
    }
    public void Restart()
    {
        isPaused = false;
    }
    public void Cancel()
    {
        _cancellationSource.Cancel();
    }
}
public static class NoNameAwaitable
{
    public static async Awaitable WaitForNextFrameAsync(NoNameToken noname)
    {
        do
        {
            await Awaitable.NextFrameAsync(noname.cancellationToken);
        }
        while (noname.isPaused);
    }

    public static async Awaitable WaitForSecondsAsync(float seconds, NoNameToken noname)
    {
        float totalTime = 0;
        while (totalTime < seconds)
        {
            await NoNameAwaitable.WaitForNextFrameAsync(noname);
            totalTime += Time.deltaTime;
        }
    }
}

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