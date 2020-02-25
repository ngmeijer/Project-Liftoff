using System;
using GXPEngine;

public class Background : GameObject
{
    private float moveSpeed = 2;

    private Sprite _background1;
    private Sprite _background2;
    private Sprite _background3;

    private int xSpawn1 = 0;
    private int xSpawn2 = 1920;
    private int xSpawn3 = 3840;

    private int ySpawn = -60;

    public Background() : base()
	{
        _background1 = new Sprite("LevelBackground1.png");
        _background2 = new Sprite("LevelBackground2.png");
        _background3 = new Sprite("LevelBackground3.png");

        AddChild(_background1);
        AddChild(_background2);
        AddChild(_background3);

        _background1.y = ySpawn;
        _background2.y = ySpawn;
        _background3.y = ySpawn;

        _background1.x = xSpawn1;
        _background2.x = xSpawn2;
        _background3.x = xSpawn3;
    }

    private void Update()
    {
        MoveBackgrounds();
        ReuseBackgrounds();
    }

    private void MoveBackgrounds()
    {
        _background1.x -= moveSpeed;
        _background2.x -= moveSpeed;
        _background3.x -= moveSpeed;
    }

    private void ReuseBackgrounds()
    {
        if(_background1.x + game.width <= 0)
        {
            _background1.x = xSpawn3;
        }

        if (_background2.x + game.width <= 0)
        {
            _background2.x = xSpawn3;
        }

        if (_background3.x + game.width <= 0)
        {
            _background3.x = xSpawn3;
        }
    }
}