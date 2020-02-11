using System;
using GXPEngine;

public class Platform : Sprite
{
	#region Variables

	private float _moveSpeed = 1;

    #endregion

    public Platform() : base("Obstacle.png", true, true)
	{
		scaleY = 0.2f;
	}

	private void Update()
	{
		MovePlatform();
	}

	private void MovePlatform()
	{
		if(y <= 100)
		{
			y += _moveSpeed;
		} 
		
		//if(y >= 800)
		//{
		//	y -= _moveSpeed;
		//}
	}
}
