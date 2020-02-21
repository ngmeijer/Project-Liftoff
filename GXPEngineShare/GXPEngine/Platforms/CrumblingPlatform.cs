using System;
using System.Collections;
using System.Collections.Generic;
using GXPEngine;

public class CrumblingPlatform : AnimationSprite
{
    #region Variables

    public float offsetX { get; private set; }
    public float offsetY { get; private set; }

    //Decrease to increase speed
    private int _animationSpeed = 50;

    private int _animationTimer;

    private int[] newPosX = { 600, 800, 1000, 1200, 1400, 1600, 1800 };

    private int spawnPositionX;
    private int spawnPositionY;
    private int newPositionX = 200;
    private int newPositionY = 200;
    public bool platformDestroyed { get; set; }

    private int animFrame;
    private int timeBeforeRespawn;
    private int timeToRespawn = 200;

    public bool changePosition { get; private set; }

    #endregion

    public CrumblingPlatform() : base("PlatformCrumblingSpritesheet.png", 10, 1)
    {
        scale = 0.4f;

        offsetX = Utils.Random(-200, 400);
        offsetY = Utils.Random(-200, 400);
    }

    private void Update()
    {
        timeBeforeRespawn++;
        Console.WriteLine(timeBeforeRespawn);

        if(timeBeforeRespawn>= timeToRespawn)
        {
            x = spawnPositionX;
            y = spawnPositionY;
            animFrame = 0;
            timeBeforeRespawn = 0;
        }
    }

    public void handleCrumbleAnimation()
    {
        _animationTimer += Time.deltaTime;
        int frame = (int)(_animationTimer / _animationSpeed) % 10 + 5;

        animFrame = frame;

        SetFrame(frame);

        if (frame >= 9)
        {
            x = -200;
        }
    }

    public void SetSpawnPosition(int xPos, int yPos)
    {
        x = xPos;
        y = yPos;

        spawnPositionX = xPos;
        spawnPositionY = yPos;
    }
}
