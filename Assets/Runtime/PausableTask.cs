using UnityEngine;

public static class PausableTask
{
    public static async Awaitable WaitForNextFrameAsync(PausableToken noname)
    {
        do
        {
            await Awaitable.NextFrameAsync(noname.cancellationToken);
        }
        while (noname.isPaused);
    }

    public static async Awaitable WaitForSecondsAsync(float seconds, PausableToken noname)
    {
        float totalTime = 0;
        while (totalTime < seconds)
        {
            await WaitForNextFrameAsync(noname);
            totalTime += Time.deltaTime;
        }
    }
}
