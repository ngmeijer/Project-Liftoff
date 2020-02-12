using System;
using GXPEngine;

public class MyGame : Game
{
    #region Variables

    private Menu _menu;
    private Level _level;

    #endregion

    public MyGame() : base(1920, 1080, false)
    {
        _menu = new Menu(_level);
        AddChild(_menu);
    }

    static void Main()
    {
        new MyGame().Start();
    }
}