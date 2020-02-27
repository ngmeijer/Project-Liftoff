using System;
using GXPEngine;

public class Menu : GameObject
{
    #region Variables

    private StartButton _startButton;

    private SceneManager _sceneManager;
    private Cutscene _cutscene;
    private Sound _backgroundMusic;
    private Sprite _logo;
    private Sprite _menuBackground;

    public Sprite solo { get; set; }
    public Sprite duo { get; set; }

    public bool levelStarted;
    public bool clickedStart { get; private set; }

    private bool startPressed;

    public bool destroyMenu { get; set; }

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

        _menuBackground = new Sprite("MenuBackground.png");
        AddChild(_menuBackground);

        _logo = new Sprite("Logo.png", true, false);
        AddChild(_logo);
        _logo.scale = 0.4f;
        _logo.x = (game.width / 2) - (_logo.width / 2);
        _logo.y = 400;

        _startButton = new StartButton();
        AddChild(_startButton);
        _startButton.x = (game.width / 2) - (_startButton.width / 2);
        _startButton.y = 800;

        solo = new Sprite("OnePlayerButton.png");
        AddChild(solo);
        solo.scale = 0.75f;
        solo.x = game.width / 2 - (solo.width + 200);
        solo.y = 630;
        solo.visible = false;

        duo = new Sprite("TwoPlayerButton.png");
        AddChild(duo);
        duo.scale = 0.75f;
        duo.x = game.width / 2 + 100;
        duo.y = 630;
        duo.visible = false;

        _backgroundMusic = new Sound("MenuTheme.mp3", true, true);
        _backgroundMusic.Play(false);
    }

    private void Update()
    {
        CheckStartInput();

        if (destroyMenu)
        {
            LateDestroy();
        }

        if (clickedStart)
        {
            solo.visible = true;
            duo.visible = true;
        }
        else if (!clickedStart)
        {
            solo.visible = false;
            duo.visible = false;
            _startButton.visible = true;
        }
    }

    #endregion

    //Checks if the start button has been pressed.
    private void CheckStartInput()
    {
        //USE THIS CODE FOR KEYBOARD
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
                _startButton.visible = false;
                clickedStart = true;
            }

            if (!startCutscene)
            {
                if (!levelStarted)
                {
                    if (soloPlayer || duoPlayers)
                    {
                        solo.visible = false;
                        duo.visible = false;
                        startCutscene = true;
                    }
                }
            }


            //USE THIS CODE FOR THE CONTROLLER
            //if (Input.GetKey(Key.B))
            //{
            //    _startButton.visible = false;
            //    clickedStart = true;

            //    startPressed = true;
            //}


            //if (startPressed)
            //{
            //    if (Input.GetKeyDown(Key.LEFT_CTRL))
            //    {
            //        soloPlayer = true;
            //        duoPlayers = false;
            //        startPressed = false;
            //    }

            //    if (Input.GetKeyDown(Key.RIGHT_CTRL))
            //    {
            //        soloPlayer = false;
            //        duoPlayers = true;
            //        startPressed = false;
            //    }

            //    if (soloPlayer || duoPlayers)
            //    {
            //        solo.visible = false;
            //        duo.visible = false;
            //        startCutscene = true;
            //    }
            //}
        }
    }
}