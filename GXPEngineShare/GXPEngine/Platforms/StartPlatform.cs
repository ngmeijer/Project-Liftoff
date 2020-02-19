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
		if(level.sceneTime > 300)
		{
			LateDestroy();
		}
	}
}
