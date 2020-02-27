using System;
using GXPEngine;

public class Powerup : Sprite
{

    //Script references
    Level level;

    //Floats
    public float offsetX;
    public float offsetY;

    //Integers
    private int timer = 0;
    private int maxTimeHidden = 150;

    //Bools
    private bool hidden = false;
    private bool relocate;

    //SFX
    private Sound _pickupSound;

    public Powerup(Level levelScript) : base("PickUp.png")
    {
        level = levelScript;

        scale = 0.75f;
        SetOrigin(width / 2, height / 2);

        offsetX = Utils.Random(200, 500);
        offsetY = Utils.Random(150, 400);

        _pickupSound = new Sound("PickupSFX.wav", false, false);
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
            timer++;
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
                _pickupSound.Play();
                timer = 0;
                level._player1 = hitInfo as Player1;
                if (level._player1.pickupsCollected <= 2)
                {
                    level._player1.pickupsCollected++;
                }
                relocate = true;
                hidden = true;
                MakeInvisible();
                ChangePosition();
                relocate = false;
            }

            if (hitInfo is Player2)
            {
                _pickupSound.Play();
                timer = 0;
                level._player2 = hitInfo as Player2;
                if (level._player2.pickupsCollected <= 2)
                {
                    level._player2.pickupsCollected++;
                }
                relocate = true;
                hidden = true;
                MakeInvisible();
                ChangePosition();
                relocate = false;
            }
        }
    }
}
