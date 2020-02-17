using System;
using GXPEngine;

class PlayerCollider : Sprite
{
    Player1 player1;
    Level level;

    public PlayerCollider(Player1 playerScript, Level levelScript) : base("Collider.png", true, true)
    {
        player1 = playerScript;

        y = player1.y - 200;
    }

    private void Update()
    {
        CheckForPlatformCollision();
    }

    private void OnCollision(GameObject other)
    {
        if (!player1._standingOnStart)
        {
            if (other is NormalPlatform)
            {
                player1.jumpCount = 0;
                player1._normalPlatform = other as NormalPlatform;
                if (!player1._playerIsMoving)
                {
                    x = player1._normalPlatform.x;
                }
                player1.y = player1._normalPlatform.y - player1._offset;
            }
        }

        if (!player1._standingOnStart)
        {
            if (other is FallingPlatform)
            {
                player1.jumpCount = 0;
                player1._fallingPlatform = other as FallingPlatform;
                if (!player1._playerIsMoving)
                {
                    x = player1._fallingPlatform.x + 50;
                }
                y = player1._fallingPlatform.y - player1._offset;
            }
        }
        if (other is StartPlatform)
        {
            player1.jumpCount = 0;
            player1._startPlatform = other as StartPlatform;
            player1._standingOnStart = true;
            y = player1._startPlatform.y - player1._offset;
        }

        if (other is Pickup)
        {
            player1.pickupsCollected += 1;
            player1.pickupScore += player1.pickupPoints;
        }
    }

    private void CheckForPlatformCollision()
    {
        if (player1._standingOnPlatform)
        {
            player1._stillStandingOnPlatform = HitTest(player1._normalPlatform);

            if (player1._stillStandingOnPlatform)
            {
                if (!player1._isJumping)
                {
                    x = player1._normalPlatform.x;
                    y = player1._normalPlatform.y;
                }
            }
            else if (!player1._stillStandingOnPlatform)
            {
                player1._standingOnPlatform = false;
            }
        }

        if (player1._standingOnPlatform)
        {
            player1._stillStandingOnFallingPlatform = HitTest(player1._fallingPlatform);

            if (player1._stillStandingOnPlatform)
            {
                if (!player1._isJumping)
                {
                    x = player1._fallingPlatform.x;
                    y = player1._fallingPlatform.y;
                }
            }
            else if (!player1._stillStandingOnPlatform)
            {
                player1._standingOnPlatform = false;
            }
        }
    }
}

