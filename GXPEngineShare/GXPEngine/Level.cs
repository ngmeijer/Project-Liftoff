using System;
using GXPEngine;
using GXPEngine.Core;

public class Level : GameObject
{
    #region Variables

    private GroundCollider _groundCollider;
    private Player _player;
    private Platform[] _platformArray;

    #endregion

    public Level()
	{
        InitializeGround();
        InitializePlayer();
        InitializePlatforms();
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
                y = Utils.Random(0, game.height)
            };
            AddChild(_platformArray[count]);
        }
    }
}
