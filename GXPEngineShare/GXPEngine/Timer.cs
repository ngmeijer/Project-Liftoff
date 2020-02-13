using System;
using GXPEngine;

public class Timer : GameObject
{
    public int TimeToWait;

	public Timer(int waitTime)
	{
        TimeToWait = waitTime;
	}

    private void Update()
    {
        TimeToWait = Time.time;
    }
}
