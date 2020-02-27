using System;
using GXPEngine;
public class InkaWhip : Sprite
{
    private Level level;

    public bool playerIsStunned { get; private set; }

    public InkaWhip(Level levelScript) : base("inka_whip.png", true, true)
    {
        level = levelScript;

        scale = 1.2f;
    }

    private void Update()
    {
        DetectPlayerCollision();
    }

    private void DetectPlayerCollision()
    {
        if (visible)
        {
            if (HitTest(level._player1))
            {
                playerIsStunned = true;
            }
        }

        if (!HitTest(level._player1))
        {
            playerIsStunned = false;
        }
    }
}