using System;
using GXPEngine;

public class Pickup : Sprite 
{
	public float offsetX;
	public float offsetY;

	Level level;

	public Pickup(Level levelScript) : base("PickUp.png")
	{
		level = levelScript;

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
		if(hitInfo is Player1)
		{
			level._player1 = hitInfo as Player1;
			level._player1.pickupsCollected++;
			LateDestroy();
		}
	}
}
