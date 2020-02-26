using System;
using GXPEngine;

public class Heart : Sprite
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

    private Sound _heartSound;

    public Heart(Level levelScript) : base("Heart.png")
    {
        level = levelScript;

        scale = 0.1f;
        SetOrigin(width / 2, height / 2);

        offsetX = Utils.Random(200, 500);
        offsetY = Utils.Random(150, 400);

        _heartSound = new Sound("PickupSFX.wav", false, false);
    }

    public void SetSpawnPosition(float xPos, float yPos)
    {
        x = xPos;
        y = yPos;
    }

    private void Update()
    {
        timer++;

        if (hidden)
        {
            timer++;
            MakeInvisible();
        }
    }

    private void MakeInvisible()
    {
        if (hidden)
        {
            this.visible = false;
        }

        if (timer >= maxTimeHidden)
        {
            hidden = false;
            this.visible = true;
            timer = 0;
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
                _heartSound.Play();
                timer = 0;
                level._player1 = hitInfo as Player1;
                level._player1.heartCollected = true;
                relocate = true;
                hidden = true;
                MakeInvisible();
                ChangePosition();
                relocate = false;
            }

            if (hitInfo is Player2)
            {
                timer = 0;
                level._player2 = hitInfo as Player2;
                level._player2.heartCollected = true;
                hidden = true;
                relocate = true;
            }
        }
    }
}
