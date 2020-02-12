using System;
using GXPEngine;

public class Background : Sprite
{
	public Background() : base("Background.png")
	{   
	}

    private void Update()
    {
        x -= 5;
    }
}
