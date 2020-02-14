using System;
using GXPEngine;

public class Pickup : Sprite 
{
	public Pickup() : base("PickUp.png")
	{
		scale = 0.5f;
		SetOrigin(width / 2, height / 2);
	}

	private void OnCollision(GameObject hitInfo)
	{
		if((hitInfo is Player1) || (hitInfo is Player2))
		{
			LateDestroy();
		}
	}
}
