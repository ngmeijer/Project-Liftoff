using System;
using GXPEngine;

public class MyGame : Game
{
    #region Variables

    public Menu _menu { get; private set; }
    public Cutscene _cutscene { get; private set; }

    private SceneManager sceneManager;

    #endregion

    public MyGame() : base(1920, 1080, false, true)
    {
        targetFps = 60;

        sceneManager = new SceneManager(_menu, _cutscene);
        AddChild(sceneManager);
    }

    static void Main()
    {
        new MyGame().Start();
    }
}