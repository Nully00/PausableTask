using Cysharp.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class PausableToken
{
    public bool isPaused => _isPausedFlag() || _isPausedToggle;
    public CancellationToken cancellationToken { get; private set; }

    public CancellationTokenSource cancellationSource { get; private set; }

    private Func<bool> _isPausedFlag = () => false;
    private bool _isPausedToggle;

    public PausableToken(MonoBehaviour mono, Func<bool> isPauseCondition = null)
    {
        cancellationSource = CancellationTokenSource.CreateLinkedTokenSource(
            new CancellationTokenSource().Token, mono.GetCancellationTokenOnDestroy());
        cancellationToken = cancellationSource.Token;

        _isPausedFlag = isPauseCondition ?? (() => false);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Pause()
    {
        _isPausedToggle = true;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Restart()
    {
        _isPausedToggle = false;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Cancel()
    {
        cancellationSource.Cancel();
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void LinkedTokenSource(CancellationToken cancellationToken)
    {
        cancellationSource = CancellationTokenSource.CreateLinkedTokenSource(
            this.cancellationToken, cancellationToken);
        this.cancellationToken = cancellationSource.Token;
    }
}
