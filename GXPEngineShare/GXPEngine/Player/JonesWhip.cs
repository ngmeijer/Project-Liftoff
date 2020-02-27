using System;
using GXPEngine;
public class JonesWhip : Sprite
{
    private Level level;
    public bool playerIsStunned { get; private set; }

    public JonesWhip(Level levelScript) : base("jones_whip.png", true, true)
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
        if (level._player2 != null)
        {
            if (visible)
            {
                if (HitTest(level._player2))
                {
                    playerIsStunned = true;
                }
            }

            if (!HitTest(level._player2))
            {
                playerIsStunned = false;
            }
        }
    }
}