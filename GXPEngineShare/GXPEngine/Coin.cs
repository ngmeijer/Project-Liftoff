using System;
using GXPEngine;

public class Coin : Sprite 
{
	public Coin() : base("Coin.png")
	{
		scale = 0.5f;
		SetOrigin(width / 2, height / 2);
	}

	private void OnCollision(GameObject hitInfo)
	{
		if(hitInfo is Player1)
		{
			LateDestroy();
		}
	}
}
