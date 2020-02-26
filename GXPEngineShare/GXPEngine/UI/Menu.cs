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
        startCutscene = false;

        _backgroundImage = new Sprite("Logo.png");
        AddChild(_backgroundImage);
        _backgroundImage.scale = 2;
        _backgroundImage.x = (game.width - _backgroundImage.width) / 2;
        _backgroundImage.y = 300;

        _startButton = new StartButton();
        AddChild(_startButton);
        _startButton.x = (game.width / 2) - (_startButton.width / 2);
        _startButton.y = 800;

        solo = new Sprite("OnePlayerButton.png");
        AddChild(solo);
        solo.scale = 0.75f;
        solo.x = game.width / 2 - (solo.width + 200);
        solo.y = 630;

        duo = new Sprite("TwoPlayerButton.png");
        AddChild(duo);
        duo.scale = 0.75f;
        duo.x = game.width / 2 + 100;
        duo.y = 630;

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