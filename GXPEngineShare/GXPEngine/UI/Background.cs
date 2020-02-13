using System;
using GXPEngine;

public class Background : Sprite
{
    private float moveSpeed = 1;

	public Background() : base("Background.png")
	{   
	}

    private void Update()
    {
        x -= moveSpeed;

        //Deez nutz
        //moveSpeed *= 1.00f;

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
