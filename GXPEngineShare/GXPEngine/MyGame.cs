using System;
using GXPEngine;

public class MyGame : Game
{
    #region Variables

    public Menu _menu { get; private set; }
    private Level _level;

    private Camera camera;

    #endregion

    public MyGame() : base(1920, 1080, false, true)
    {
        _menu = new Menu(_level);
        AddChild(_menu);
    }

    static void Main()
    {
        new MyGame().Start();
    }
}