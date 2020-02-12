using System;
using GXPEngine;
using GXPEngine.Core;

public class Level : GameObject
{
    #region Variables

    private GroundCollider _groundCollider;
    private Player _player;
    private Platform[] _platformArray;
    private Background[] _backgroundArray;
    private Background _background;
    private Background _background2;

    #endregion

    public Level()
	{
        InitializeBackground();
        InitializeGround();
        InitializePlayer();
        InitializePlatforms();
	}
    
    private void InitializeBackground()
    {
        _background = new Background();
        _background2 = new Background();
        AddChild(_background);
        AddChild(_background2);

        _background2.x = game.width;
    }

    private void InitializeGround()
    {
        _groundCollider = new GroundCollider
        {
            y = game.height - 64
        };
        AddChild(_groundCollider);
    }

    private void InitializePlayer()
    {
        _player = new Player();
        AddChild(_player);
    }

    private void InitializePlatforms()
    {
        _platformArray = new Platform[25];
        for (int count = 0; count < _platformArray.Length; count++)
        {
            if (_platformArray[count] == null)
            {
                _platformArray[count] = new Platform
                {
                    x = Utils.Random(0, game.width),
                    y = Utils.Random(100, 800),
                };
                AddChild(_platformArray[count]);
                _platformArray[count].x += 1920;
            }
        }
    }

    private void InitializeHUD()
    {
        PauseMenu pauseMenu = new PauseMenu();
        AddChild(pauseMenu);
    }
}
