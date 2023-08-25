using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class AsyncTest : MonoBehaviour
{
    private async void Start()
    {
        Debug.Log("Start");
        PausableToken token = new PausableToken(this);
        _ = TestAsync(token);

        Destroy(this.gameObject);
        //Pause����

        //token.Pause();
        //await Awaitable.WaitForSecondsAsync(2.0f);
        //token.Restart();



        //Cancel����
        //token.Cancel();


        Debug.Log("End");
    }

    private async Awaitable TestAsync(PausableToken pausableToken)
    {
        Debug.Log("TestAsyncStart");
        await PausableTask.WaitForSecondsAsync(1.0f, pausableToken);
        Debug.Log("TestAsyncEnd");
    }
}