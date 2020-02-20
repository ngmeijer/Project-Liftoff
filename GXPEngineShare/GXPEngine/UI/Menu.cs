using System;
using GXPEngine;

public class Menu : GameObject
{
    #region Variables

    private StartButton _startButton;

    private Level _newLevel;
    private CutscenePart1 _cutscene1;
    private CutscenePart2 _cutscene2;
    private SoundChannel _soundManager;
    private Sound _backgroundMusic;
    private Sprite _backgroundImage;

    private Sprite solo;
    private Sprite duo;

    public bool levelStarted;

    public bool soloPlayer { get; private set; }
    public bool duoPlayers { get; private set; }

    #endregion

    #region Constructor & Update method

    public Menu(Level level, CutscenePart1 cutscene) : base()
    {
        _cutscene1 = cutscene;
        _newLevel = level;

        levelStarted = false;

        _backgroundImage = new Sprite("Logo.png");
        AddChild(_backgroundImage);
        _backgroundImage.scale = 2;
        _backgroundImage.x = (game.width - _backgroundImage.width) / 2;
        _backgroundImage.y = 300;

        _startButton = new StartButton();
        AddChild(_startButton);
        _startButton.x = (game.width - _startButton.width) / 2;
        _startButton.y = 800;

        solo = new Sprite("GroundCollider.png");
        AddChild(solo);
        solo.scale = 2;
        solo.x = 600;
        solo.y = game.height / 2;

        duo = new Sprite("GroundCollider.png");
        AddChild(duo);
        duo.color = 255;
        duo.scale = 2;
        duo.x = 1400;
        duo.y = game.height / 2;

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
        //if (!levelStarted)
        //{
        //    _newLevel = new Level(this);

        //    if (duoPlayers)
        //    {
        //        _newLevel.duo = true;
        //    }

        //    AddChild(_newLevel);
        //    levelStarted = true;
        //    _cutscene = null;
        //}

        _cutscene1 = new CutscenePart1();
        AddChild(_cutscene1);

        if (_cutscene1.cutsceneFinished)
        {
            _cutscene2 = new CutscenePart2();
            AddChild(_cutscene2);
            RemoveChild(_cutscene1);
        }
    }

    //Checks if the start button has been pressed.
    private void CheckStartInput()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (solo.HitTestPoint(Input.mouseX, Input.mouseY))
            {
                soloPlayer = true;
                duoPlayers = false;
            }

            if (duo.HitTestPoint(Input.mouseX, Input.mouseY))
            {
                soloPlayer = false;
                duoPlayers = true;
            }

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