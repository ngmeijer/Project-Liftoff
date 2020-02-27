using System;
using GXPEngine;
public class JonesWhip : Sprite
{
    private Level level;
    private bool hittingScreenBorder;

    public JonesWhip(Level levelScript) : base("jones_whip.png", true, true)
    {
        level = levelScript;

        scale = 1.2f;
    }
}