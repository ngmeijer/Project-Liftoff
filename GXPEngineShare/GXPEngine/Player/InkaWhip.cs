using System;
using GXPEngine;
public class InkaWhip : Sprite
{
    private Level level;
    private bool hittingScreenBorder;

    private ScreenBorders _screenBorder;
    

    public InkaWhip(Level levelScript) : base("inka_whip.png", true, true)
    {
        level = levelScript;

        scaleX = 20f;
    }

    private void Update()
    {
        if (level._player2.whipSprite.visible)
        {
            if (hittingScreenBorder)
            {
                Console.WriteLine("true");
                level._player2.flyToBorder = true;
            }
        }
        else
        {
            level._player2.flyToBorder = false;
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