using System;
using GXPEngine;

public class MyGame : Game
{
    #region Variables

    public Menu _menu { get; private set; }
    private Level _level;
    private Cutscene _cutscene;

    #endregion

    public MyGame() : base(1920, 1080, false, true)
    {
        _menu = new Menu(_level, _cutscene);
        AddChild(_menu);

        if (_menu.levelStarted)
        {
            _level = new Level(_menu);
            AddChild(_level);
            _menu.levelStarted = false;
            RemoveChild(_menu);
        }
    }

    static void Main()
    {
        new MyGame().Start();
    }
}