using System;
using GXPEngine;

public class Pickup : Sprite
{
    public float offsetX;
    public float offsetY;

    Level level;
    private bool hidden = false;
    private int timer = 0;
    private int maxTimeHidden = 150;

    public int[] firstSpawnsX = { 500, 800, 1200, 1600 };
    public int[] firstSpawnsY = { 200, 450, 600, 800 };
    private bool relocate;

    public Pickup(Level levelScript) : base("PickUp.png")
    {
        level = levelScript;

        scale = 0.5f;
        SetOrigin(width / 2, height / 2);

        offsetX = Utils.Random(0, 400);
        offsetY = Utils.Random(0, 250);
    }

    public void SetSpawnPosition(float xPos, float yPos)
    {
        x = xPos;
        y = yPos;
    }

    private void Update()
    {
        MakeInvisible();
    }

    private void MakeInvisible()
    {
        timer++;

        if (hidden)
        {
            this.visible = false;
            if (timer >= maxTimeHidden)
            {
                this.visible = true;
            }
        }
        else if (!hidden)
        {
            this.visible = true;
        }
    }

    public void ChangePosition()
    {
        if (relocate)
        {
            if (x >= game.width / 2)
            {
                x -= offsetX;
            }
            else if (x <= game.width / 2)
            {
                x += offsetX;
            }

            if (y >= game.height / 2)
            {
                y -= offsetY;
            }
            else if (y <= game.height / 2)
            {
                y += offsetY;
            }

            relocate = false;
        }
    }

    private void OnCollision(GameObject hitInfo)
    {
        if (!hidden)
        {
            if (hitInfo is Player1)
            {
                timer = 0;
                level._player1 = hitInfo as Player1;
                level._player1.pickupsCollected++;
                relocate = true;
                ChangePosition();
                relocate = false;
            }

            if (hitInfo is Player2)
            {
                timer = 0;
                level._player2 = hitInfo as Player2;
                level._player2.pickupsCollected++;
                hidden = true;
                relocate = true;
            }
        }
    }
}
