using System;
using GXPEngine;

public class Pickup : Sprite 
{
	public float offsetX;
	public float offsetY;

	public Pickup() : base("PickUp.png")
	{
		scale = 0.5f;
		SetOrigin(width / 2, height / 2);

		offsetX = Utils.Random(0, 600);
		offsetY = Utils.Random(-300, 300);
	}
	public void SetSpawnPosition(float xPos, float yPos)
	{
		x = xPos;
		y = yPos;
	}

	private void OnCollision(GameObject hitInfo)
	{
		if((hitInfo is Player1) || (hitInfo is Player2))
		{
			LateDestroy();
		}
	}
}
