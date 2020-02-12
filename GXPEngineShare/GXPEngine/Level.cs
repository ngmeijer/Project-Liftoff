﻿using System;
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

    #endregion

    public Level()
	{
        InitializeHUD();
        InitializeBackground();
        InitializePlayer();
        InitializePlatforms();
	}

    private void Update()
    {
        if (Input.GetKeyDown(Key.TAB))
        {
            _pauseMenu.visible = true;
        }
    }
    
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

    private void InitializeHUD()
    {
        _pauseMenu = new PauseMenu();
        AddChild(_pauseMenu);

        _pauseMenu.visible = false;
    }
}
