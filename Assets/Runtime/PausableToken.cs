using System.Threading;
using UnityEngine;

public class PausableToken
{
    public bool isPaused { get; private set; } = false;
    public CancellationToken cancellationToken { get; private set; }

    private CancellationTokenSource _cancellationSource = null;
    private MonoBehaviour _monoBehaviour;

    public PausableToken(MonoBehaviour mono)
    {
        this._monoBehaviour = mono;

        if (_cancellationSource == null)
        {
            _cancellationSource = new CancellationTokenSource();
        }
        cancellationToken = _cancellationSource.Token;

        var onDestroyC = _monoBehaviour.gameObject.AddComponent<OnDestroyCallbackComponent>();
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
