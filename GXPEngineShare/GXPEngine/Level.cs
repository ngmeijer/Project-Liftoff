using System;
using GXPEngine;
using GXPEngine.Core;

public class Level : GameObject
{
    #region Variables

    private Player _player;
    private Platform[] _platformArray;

    #endregion
    public Level()
	{
        InitializePlayer();
        InitializePlatforms();

        foreach(GameObject Platform in _platformArray)
        {
        }
	}

    private void InitializePlayer()
    {
        _player = new Player();
        AddChild(_player);
    }

    private void InitializePlatforms()
    {
        _platformArray = new Platform[5];
        for (int count = 0; count < _platformArray.Length; count++)
        {
            _platformArray[count] = new Platform
            {
                x = Utils.Random(0, game.width),
                y = Utils.Random(0, game.height)
            };
            AddChild(_platformArray[count]);
        }
    }
}
