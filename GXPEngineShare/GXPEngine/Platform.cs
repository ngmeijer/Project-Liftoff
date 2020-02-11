using System;
using GXPEngine;

public class Platform : Sprite
{
	public Platform(float xPos, float yPos) : base("Obstacle.png", true, true)
	{
        x = xPos;
        y = yPos;
	}
}
