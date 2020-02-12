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

        ReuseBackgrounds();
    }

    private void ReuseBackgrounds()
    {
        if(x + 1920 <= 0)
        {
            x = 1920;
        }
    }
}
