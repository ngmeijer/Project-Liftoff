using System;
using GXPEngine;

public class Menu : GameObject
{
    #region Variables

    private StartButton _startButton;

    private Level _newLevel;
    private SoundChannel _soundManager;
    private Sound _backgroundMusic;
    private Sprite _backgroundImage;

    public bool levelStarted;

    #endregion

    #region Constructor & Update method

    public Menu(Level level) : base()
    {
        _newLevel = level;

        _backgroundImage = new Sprite("Logo.png");
        AddChild(_backgroundImage);
        _backgroundImage.scale = 2;
        _backgroundImage.x = (game.width - _backgroundImage.width) / 2;
        _backgroundImage.y = 300;

        levelStarted = false;

        _startButton = new StartButton();
        AddChild(_startButton);
        _startButton.x = (game.width - _startButton.width) / 2;
        _startButton.y = 800;

        _backgroundMusic = new Sound("MenuTheme.mp3", true, true);
        _backgroundMusic.Play(false);
    }

    private void Update()
    {
        CheckStartInput();
        CheckLevelReset();
    }

    #endregion

    private void CheckLevelReset()
    {
        if (_newLevel != null)
        {
            if (_newLevel.resetGame)
            {
                _newLevel = null;
                levelStarted = false;
                hideShowMenu();
            }
        }
    }

    //Start the game by adding an instance of the Level class. 
    private void startGame()
    {
        if (!levelStarted)
        {
            _newLevel = new Level();
            AddChild(_newLevel);
            levelStarted = true;
        }
    }

    //Checks if the start button has been pressed.
    private void CheckStartInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_startButton.HitTestPoint(Input.mouseX, Input.mouseY))
            {
                startGame();
                hideShowMenu();
            }
        }
    }

    //This method hides the start button once clicked and makes it unclickable.
    private void hideShowMenu()
    {
        if (levelStarted)
        {
            _startButton.visible = false;
        }
        if (!levelStarted)
        {
            _startButton.visible = true;
        }
    }
}