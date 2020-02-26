﻿using System;
using GXPEngine;

public class Level : GameObject
{
    #region Variables

    //Class references
    private Powerup[] _pickupArray;
    private Heart[] _heartArray;

    private StartPlatform _startPlatform1;
    private StartPlatform _startPlatform2;
    private MovingPlatform[] _movingPlatformArray;
    private CrumblingPlatform[] _crumblingPlatformArray;
    private CrumblingPlatform[] _crumblingPlatformArray2;
    private MovingPlatform[] _movingPlatformArray2;

    public Player1 _player1 { get; set; }
    public Player2 _player2 { get; set; }
    private Background _background;
    private Background _background2;
    public HUD hud { get; private set; }
    private Menu menu;
    public ScreenBorders borders;
    private Spears spears;


    //Bools
    public bool resetGame;
    public bool playingDuo { get; set; }

    //Integers
    public int sceneTime { get; set; }
    private int fallingCount = 0;

    private int xPosNormal = 700;
    private int yPosNormal = 310;
    private int xPosNormal2 = 800;
    private int yPosNormal2 = 910;

    private int xPosFalling = 800;
    private int yPosFalling = 250;
    private int xPosFalling2 = 800;
    private int yPosFalling2 = 800;

    private int xPositionHeart = 600;
    private int yPositionHeart = 800;

    private int xPositionPowerup = 600;
    private int yPositionPowerup = 300;

    private Sprite _background1;
    private Sprite _hudBackground;

    #endregion

    public Level(Menu menuScript)
    {
        menu = menuScript;

        InitializeBackground();

        _hudBackground = new Sprite("HUDBackground.png");
        AddChild(_hudBackground);
        _hudBackground.y = 0;

        borders = new ScreenBorders();
        AddChild(borders);
        borders.y = 50;
        borders.x = 0;

        spears = new Spears();
        AddChild(spears);
        spears.y = 920;

        SpawnNewFallingPlatforms();
        InitializePlatforms();
        InitializePlayers();
        InitializePickupsAndHearts();
        InitializeHUD();
    }

    private void Update()
    {
        sceneTime++;

        if (sceneTime > 1000)
        {
            SpawnNewFallingPlatforms();
            sceneTime = 0;
        }
    }

    #region Draw level
    private void InitializeBackground()
    {
        _background = new Background();
        _background2 = new Background();
        AddChild(_background);
    }

    private void InitializePlayers()
    {
        _player1 = new Player1(100, 200, hud, this);
        AddChild(_player1);

        if (menu.duoPlayers)
        {
            _player2 = new Player2(100, 500, hud, this);
            AddChild(_player2);
        }
    }

    private void InitializePlatforms()
    {
        //Normal platforms
        _movingPlatformArray = new MovingPlatform[4];

        for (int count = 0; count < _movingPlatformArray.Length; count++)
        {
            _movingPlatformArray[count] = new MovingPlatform(this, menu);
            _movingPlatformArray[count].SetSpawnPosition(xPosNormal, yPosNormal);
            xPosNormal += 400;
            yPosNormal += 160;

            AddChild(_movingPlatformArray[count]);
        }

        _movingPlatformArray2 = new MovingPlatform[5];

        for (int count = 0; count < _movingPlatformArray2.Length; count++)
        {
            _movingPlatformArray2[count] = new MovingPlatform(this, menu);
            _movingPlatformArray2[count].SetSpawnPosition(xPosNormal2, yPosNormal2);
            xPosNormal2 += 300;
            yPosNormal2 -= 150;

            AddChild(_movingPlatformArray2[count]);
        }

        //Start platforms. Clean this up.
        _startPlatform1 = new StartPlatform(this);
        AddChild(_startPlatform1);
        _startPlatform1.y = 300;

        _startPlatform2 = new StartPlatform(this);
        AddChild(_startPlatform2);
        _startPlatform2.y = 700;
    }

    private void InitializePickupsAndHearts()
    {
        _pickupArray = new Powerup[2];

        for (int count = 0; count < _pickupArray.Length; count++)
        {
            _pickupArray[count] = new Powerup(this);
            _pickupArray[count].SetSpawnPosition(xPositionPowerup, yPositionPowerup);
            xPositionPowerup += 800;
            yPositionPowerup += 500;
            AddChild(_pickupArray[count]);
        }

        _heartArray = new Heart[2];

        for (int count = 0; count < _heartArray.Length; count++)
        {
            _heartArray[count] = new Heart(this);
            _heartArray[count].SetSpawnPosition(xPositionHeart, yPositionHeart);
            xPositionHeart += 800;
            yPositionHeart -= 500;
            AddChild(_heartArray[count]);
        }
    }

    private void InitializeHUD()
    {
        hud = new HUD(_player1, _player2, menu);
        AddChild(hud);
    }

    #endregion

    private void SpawnNewFallingPlatforms()
    {
        _crumblingPlatformArray = new CrumblingPlatform[7];

        xPosFalling = 600;

        for (int count = 0; count < _crumblingPlatformArray.Length; count++)
        {
            _crumblingPlatformArray[count] = new CrumblingPlatform();
            _crumblingPlatformArray[count].SetSpawnPosition(xPosFalling, yPosFalling);
            xPosFalling += 310;

            AddChild(_crumblingPlatformArray[count]);
        }

        xPosFalling2 = 600;

        _crumblingPlatformArray2 = new CrumblingPlatform[7];

        for (int count = 0; count < _crumblingPlatformArray2.Length; count++)
        {
            _crumblingPlatformArray2[count] = new CrumblingPlatform();
            _crumblingPlatformArray2[count].SetSpawnPosition(xPosFalling2, yPosFalling2);
            xPosFalling2 += 310;

            AddChild(_crumblingPlatformArray2[count]);
        }
    }
}
