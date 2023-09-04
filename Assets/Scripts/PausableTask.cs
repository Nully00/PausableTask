using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System;
using UnityEngine;

public static class PausableTask
{
    public static async UniTask Yield(PausableToken token)
    {
        do
        {
            await UniTask.Yield(token.cancellationToken);
        }
        while (token.isPaused);
    }
    public static async UniTask Yield(PlayerLoopTiming timing, PausableToken token)
    {
        do
        {
            await UniTask.Yield(timing, token.cancellationToken);
        }
        while (token.isPaused);
    }
    public static async UniTask WaitForSeconds(float seconds, PausableToken token)
    {
        float totalTime = 0;
        while (totalTime < seconds)
        {
            await Yield(token);
            totalTime += Time.deltaTime;
        }
    }


    public static async UniTask DelayAction(float seconds, Action action, PausableToken token)
    {
        await WaitForSeconds(seconds, token);
        try
        {
            action?.Invoke();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while executing the action.", ex);
        }
    }

    public static async UniTask TaskThenInvoke(UniTask task, Action action)
    {
        await task;
        action?.Invoke();
    }
    public static async UniTask AppendTask(params UniTask[] task)
    {
        for (int i = 0; i < task.Length; i++)
        {
            await task[i];
        }
    }
    public static async UniTask WaitUntil(Func<bool> condition, PausableToken token)
    {
        while (!condition())
        {
            await Yield(token);
        }
    }
    public static async UniTask WaitUntil(Func<bool> condition, UniTask task, PausableToken token = null)
    {
        await WaitUntil(condition, token);
        await task;
    }
    public static async UniTask WaitWhile(Func<bool> condition, PausableToken token)
    {
        while (condition())
        {
            await Yield(token);
        }
    }
    public static async UniTask WaitWhile(Func<bool> condition, UniTask task, PausableToken token = null)
    {
        await WaitWhile(condition, token);
        await task;
    }
    public static async UniTask WhenAll(params UniTask[] tasks)
    {
        await UniTask.WhenAll(tasks);
    }


    public static async UniTask ActionWhileDuration(float seconds, Action action, PausableToken token)
    {
        await ActionWhileDuration_Internal(seconds, t => action(), token);
    }
    public static async UniTask ActionWhileDuration(float seconds, Action<float> action, PausableToken token)
    {
        await ActionWhileDuration_Internal(seconds, action, token);
    }
    public static async UniTask ActionWhileDuration01(float seconds, Action<float> action, PausableToken token)
    {
        await ActionWhileDuration_Internal(seconds, t => action(t / seconds), token);
    }
    private static async UniTask ActionWhileDuration_Internal(float seconds, Action<float> action, PausableToken token)
    {
        float totalTime = 0;
        action(0);
        while (totalTime < seconds)
        {
            await Yield(token);
            totalTime += UnityEngine.Time.deltaTime;
            action(Mathf.Min(totalTime, seconds));
        }
    }
    public static async UniTask ActionWhileTask(UniTask task, Action action, PausableToken token)
    {
        bool finished = false;
        _ = TaskThenInvoke(task, () => finished = true);
        while (!finished)
        {
            action();
            await Yield(token);
        }
    }

    #region Collider,Collision
    public static async UniTask<Collider2D> OnTriggerEnter2D(AsyncTriggerEnter2DTrigger trigger, PausableToken token)
    {
        Collider2D collider2D;
        do
        {
            collider2D = await trigger.OnTriggerEnter2DAsync(token.cancellationToken);
        } while (token.isPaused);
        return collider2D;
    }

    public static async UniTask<Collider2D> OnTriggerExit2D(AsyncTriggerExit2DTrigger trigger, PausableToken token)
    {
        Collider2D collider2D;
        do
        {
            collider2D = await trigger.OnTriggerExit2DAsync(token.cancellationToken);
        } while (token.isPaused);
        return collider2D;
    }

    public static async UniTask<Collider> OnTriggerEnter(AsyncTriggerEnterTrigger trigger, PausableToken token)
    {
        Collider collider;
        do
        {
            collider = await trigger.OnTriggerEnterAsync(token.cancellationToken);
        } while (token.isPaused);
        return collider;
    }

    public static async UniTask<Collider> OnTriggerExit(AsyncTriggerExitTrigger trigger, PausableToken token)
    {
        Collider collider;
        do
        {
            collider = await trigger.OnTriggerExitAsync(token.cancellationToken);
        } while (token.isPaused);
        return collider;
    }

    public static async UniTask<Collision2D> OnCollisionEnter2D(AsyncCollisionEnter2DTrigger trigger, PausableToken token)
    {
        Collision2D collision2D;
        do
        {
            collision2D = await trigger.OnCollisionEnter2DAsync(token.cancellationToken);
        } while (token.isPaused);
        return collision2D;
    }

    public static async UniTask<Collision2D> OnCollisionExit2DAsync(AsyncCollisionExit2DTrigger trigger, PausableToken token)
    {
        Collision2D collision2D;
        do
        {
            collision2D = await trigger.OnCollisionExit2DAsync(token.cancellationToken);
        } while (token.isPaused);
        return collision2D;
    }

    public static async UniTask<Collision> OnCollisionEnterAsync(AsyncCollisionEnterTrigger trigger, PausableToken token)
    {
        Collision collision;
        do
        {
            collision = await trigger.OnCollisionEnterAsync(token.cancellationToken);
        } while (token.isPaused);
        return collision;
    }

    public static async UniTask<Collision> OnCollisionExitAsync(AsyncCollisionExitTrigger trigger, PausableToken token)
    {
        Collision collision;
        do
        {
            collision = await trigger.OnCollisionExitAsync(token.cancellationToken);
        } while (token.isPaused);
        return collision;
    }

    public static async UniTask<Collider2D> OnTriggerStay2DAsync(AsyncTriggerStay2DTrigger trigger, PausableToken token)
    {
        Collider2D collider2D;
        do
        {
            collider2D = await trigger.OnTriggerStay2DAsync(token.cancellationToken);
        } while (token.isPaused);
        return collider2D;
    }
    public static async UniTask<Collider> OnTriggerStayAsync(AsyncTriggerStayTrigger trigger, PausableToken token)
    {
        Collider collider;
        do
        {
            collider = await trigger.OnTriggerStayAsync(token.cancellationToken);
        } while (token.isPaused);
        return collider;
    }

    public static async UniTask<Collision2D> OnCollisionStay2DAsync(AsyncCollisionStay2DTrigger trigger, PausableToken token)
    {
        Collision2D collision2D;
        do
        {
            collision2D = await trigger.OnCollisionStay2DAsync(token.cancellationToken);
        } while (token.isPaused);
        return collision2D;
    }

    public static async UniTask<Collision> OnCollisionStayAsync(AsyncCollisionStayTrigger trigger, PausableToken token)
    {
        Collision collision;
        do
        {
            collision = await trigger.OnCollisionStayAsync(token.cancellationToken);
        } while (token.isPaused);
        return collision;
    }
    #endregion
}
