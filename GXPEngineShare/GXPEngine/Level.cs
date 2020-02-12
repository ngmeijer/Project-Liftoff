using System;
using GXPEngine;
using GXPEngine.Core;

public class Level : GameObject
{
    #region Variables

    private GroundCollider _groundCollider;
    private Player _player;
    private Platform[] _platformArray;
    private Background _background;

    #endregion

    public Level()
	{
        _background = new Background();
        AddChild(_background);

        InitializeGround();
        InitializePlayer();
        InitializePlatforms();
	}

    private void Update()
    {

    }


    private void InitializeGround()
    {
        _groundCollider = new GroundCollider();
        _groundCollider.y = game.height - 64;
        AddChild(_groundCollider);
    }

    private void InitializePlayer()
    {
        _player = new Player();
        AddChild(_player);
    }

    private void InitializePlatforms()
    {
        _platformArray = new Platform[5];
        for (int count = 0; count < _platformArray.Length; count++)
        {
            _platformArray[count] = new Platform
            {
                x = Utils.Random(0, game.width),
                y = Utils.Random(100, 800)
            };
            AddChild(_platformArray[count]);
        }
    }
}
