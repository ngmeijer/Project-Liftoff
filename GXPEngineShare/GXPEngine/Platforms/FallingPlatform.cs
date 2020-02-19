using System;
using System.Collections;
using System.Collections.Generic;
using GXPEngine;

public class FallingPlatform : AnimationSprite
{
	#region Variables

    public float offsetX;
    public float offsetY;

    //Decrease to increase speed
    private int _animationSpeed = 50;

    private int _animationTimer;

    private int[] newPosX = { 600, 800, 1000, 1200, 1400, 1600, 1800 };
    public bool platformDestroyed { get; set; }

    private int timeCounter = 0;
    private int movedDistance = 0;

    #endregion

    public FallingPlatform() : base("PlatformCrumblingSpritesheet.png", 10, 1)
	{
        scale = 0.4f;

        offsetX = Utils.Random(-600, 500);
        offsetY = Utils.Random(-600, 300);
    }

	private void Update()
	{
        timeCounter++;

        if(timeCounter > 600)
        {
            y += 10;
        }



        KillPlatforms();
	}

    public void handleCrumbleAnimation()
    {
        _animationTimer += Time.deltaTime;
        int frame = (int)(_animationTimer / _animationSpeed) % 10 + 5;

        SetFrame(frame);

        if(frame >= 10)
        {
            LateRemove();
        }
    }

    public void SetSpawnPosition(float xPos, float yPos)
    {
        x = xPos;
        y = yPos;
    }

    private void KillPlatforms()
    {
        if(y > game.height)
        {
            LateRemove();
        }
    }
}
