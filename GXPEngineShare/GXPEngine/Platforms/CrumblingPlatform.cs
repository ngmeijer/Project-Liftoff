using System;
using System.Collections;
using System.Collections.Generic;
using GXPEngine;

public class CrumblingPlatform : AnimationSprite
{
    #region Variables

    //Integers
    private int sceneTime;
    private int timeBeforeRespawn = 1000;
    private int movedDistance = 0;
    private int _animationSpeed = 250;
    private int _animationTimer;

    //Floats
    public float offsetX { get; private set; }
    public float offsetY { get; private set; }

    //Decrease to increase speed
    public bool platformDestroyed { get; set; }
    private bool playerOnPlatform;
    public bool changePosition { get; private set; }

    #endregion

    public CrumblingPlatform() : base("Platform_Crumbling_Spritesheet.png", 10, 1)
    {
        SetScaleXY(0.5f, 0.5f);

        offsetX = Utils.Random(-200, 400);
        offsetY = Utils.Random(-200, 400);
    }
    private void Update()
    {
        HandleIntroSpeed();
        DestroyPlatform();
    }

    private void HandleIntroSpeed()
    {
        int moveSpeed = 1;
        int maxMovedDistance = 600;
        movedDistance += moveSpeed;

        if (movedDistance < maxMovedDistance)
        {
            x -= moveSpeed;
        }
    }

    public void handleCrumbleAnimation()
    {
        _animationTimer += Time.deltaTime;
        int frame = (int)(_animationTimer / _animationSpeed) % 10 + 5;

        SetFrame(frame);

        if(frame >= 10)
        {
            LateDestroy();
        }
    }

    private void DestroyPlatform()
    {
        sceneTime++;

        if (sceneTime > timeBeforeRespawn)
        {
            LateDestroy();
        }
    }

    public void SetSpawnPosition(int xPos, int yPos)
    {
        x = xPos;
        y = yPos;
    }
}
