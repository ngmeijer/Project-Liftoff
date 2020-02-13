using System;
using GXPEngine;

public class Background : Sprite
{
    private float moveSpeed = 1;

	public Background() : base("PlaceholderBackground.png")
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
        if(x + game.width <= 0)
        {
            x = game.width;
        }
    }
}
