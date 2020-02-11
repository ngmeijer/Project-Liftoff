using System;
using GXPEngine;

public class MyGame : Game
{
    #region Variables

    private Level _level;

    #endregion

    public MyGame() : base(1920, 1080, true)
    {
        _level = new Level();
        AddChild(_level);
    }

    static void Main()
    {
        new MyGame().Start();
    }
}