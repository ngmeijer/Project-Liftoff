using System;
using GXPEngine;

public class StartPlatform : Sprite
{
	public StartPlatform() : base("GroundCollider.png", false, true)
	{
        scaleX = 5;
		scaleY = 0.5f;
	}
}
