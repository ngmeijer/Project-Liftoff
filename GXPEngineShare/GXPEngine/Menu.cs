using System;
using GXPEngine;

public class Menu : GameObject
{
    #region Variables

    private StartButton _startButton;

    private Level _newLevel;

    private Sound _backgroundMusic;
    private Sprite _backgroundImage;

    public bool levelStarted;

    #endregion

    #region Constructor & Update method

    public Menu(Level level) : base()
    {
        _newLevel = level;

        levelStarted = false;

        _startButton = new StartButton();
        AddChild(_startButton);
        _startButton.x = (game.width - _startButton.width) / 2;
        _startButton.y = (game.height - _startButton.height) / 2;

        //_backgroundMusic = new Sound("", true, false);
        //_backgroundMusic.Play(false, 3);
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