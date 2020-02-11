using System;
using GXPEngine;
using GXPEngine.Core;

public class Level : GameObject
{
    #region Variables

    private Player _player;
    private Platform _examplePlatform;

    #endregion
    public Level()
	{
        InitializePlayer();
        InitializePlatforms();
        AddToCanvas();
	}

    private void InitializePlayer()
    {
        _player = new Player();
    }

    private void InitializePlatforms()
    {
        _examplePlatform = new Platform(game.width / 2, 500);
    }

    private void AddToCanvas()
    {
        AddChild(_player);
        AddChild(_examplePlatform);
    }
}
