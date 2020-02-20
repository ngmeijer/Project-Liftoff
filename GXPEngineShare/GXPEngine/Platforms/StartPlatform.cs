using System;
using GXPEngine;

public class StartPlatform : Sprite
{
	Level level;
	public StartPlatform(Level levelScript) : base("Start_Platform.png", false, true)
	{
		scaleY = 0.65f;
		level = levelScript;
    }

	private void Update()
	{
		//x -= 2;

		if(x < 0 - width)
		{
			LateDestroy();
		}
	}
}
