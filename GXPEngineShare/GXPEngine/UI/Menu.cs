using System;
using GXPEngine;

public class Menu : GameObject
{
    #region Variables

    private StartButton _startButton;

    private SceneManager _sceneManager;
    private Cutscene _cutscene;
    private Sound _backgroundMusic;
    private Sprite _backgroundImage;

    private Sprite solo;
    private Sprite duo;

    public bool levelStarted;
    public bool startCutscene { get; set; }

    public bool soloPlayer { get; private set; }
    public bool duoPlayers { get; private set; }

    #endregion

    #region Constructor & Update method

    public Menu(SceneManager sceneManager, Cutscene cutscene) : base()
    {
        _sceneManager = sceneManager;
        _cutscene = cutscene;

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
    }

    #endregion

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

            if (!startCutscene)
            {
                if (!levelStarted)
                {
                    if (_startButton.HitTestPoint(Input.mouseX, Input.mouseY))
                    {
                        startCutscene = true;
                        hideShowMenu();
                    }
                }
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