using System;
using GXPEngine;
using GXPEngine.Core;

public class Level : GameObject
{
    #region Variables

    private StartPlatform _groundCollider;
    private Player _player;
    private Platform[] _platformArray;
    private Background[] _backgroundArray;
    private Background _background;
    private Background _background2;
    private StartPlatform _startPlatform;
    private PauseMenu _pauseMenu;
    private Coin[] _coinArray;

    private HUD hud;
    public bool resetGame;

    #endregion

    public Level()
	{
        InitializeBackground();
        InitializePlayer();
        InitializeHUD();
        InitializePlatforms();
        InitializeCoins();
	}

    private void Update()
    {
        if (Input.GetKeyDown(Key.TAB))
        {
            _pauseMenu.visible = true;
        }
    }


    #region Draw level

    private void InitializeBackground()
    {
        _background = new Background();
        _background2 = new Background();
        AddChild(_background);
        AddChild(_background2);

        _background2.x = game.width;
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

        _startPlatform = new StartPlatform();
        AddChild(_startPlatform);
        _startPlatform.y = game.height / 2;
    }

    private void InitializeCoins()
    {
        _coinArray = new Coin[5];
        for (int count = 0; count < _coinArray.Length; count++)
        {
            if (_coinArray[count] == null)
            {
                _coinArray[count] = new Coin
                {
                    x = Utils.Random(0, game.width),
                    y = Utils.Random(100, 800),
                };
                AddChild(_coinArray[count]);
            }
        }
    }

    private void InitializeHUD()
    {
        hud = new HUD(_player);
        AddChild(hud);
    }

    #endregion

    private void CheckGameReset()
    {
        if (_player.GetLifeCount() <= 0)
        {
            resetGame = true;
            Destroy();
        }
        else
        {
            resetGame = false;
        }
    }
}
