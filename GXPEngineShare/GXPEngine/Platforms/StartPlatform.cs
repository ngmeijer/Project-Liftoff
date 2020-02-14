using System;
using GXPEngine;

public class StartPlatform : Sprite
{
	public StartPlatform() : base("Start_Platform.png", false, true)
	{

    }

    private void Update()
    {
        if (Time.time > 10000)
        {
            LateDestroy();
        }
    }
}
