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

        scale = 1.2f;
    }
}