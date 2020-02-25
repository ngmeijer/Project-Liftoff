﻿using System;
using GXPEngine;
public class JonesWhip : Sprite
{
    private Level level;
    private bool hittingScreenBorder;

    private ScreenBorders _screenBorder;
    

    public JonesWhip(Level levelScript) : base("jones_whip.png", true, true)
    {
        level = levelScript;

        scaleX = 10f;
    }

    private void Update()
    {
        if (level._player1.whipSprite.visible)
        {
            if (hittingScreenBorder)
            {
                Console.WriteLine("true");
                level._player1.flyToBorder = true;
            }
        }
        else
        {
            level._player1.flyToBorder = false;
        }
    }

    private void OnCollision(GameObject other)
    {
        if(other is ScreenBorders)
        {
            _screenBorder = other as ScreenBorders;
            hittingScreenBorder = true;
        }
    }
}