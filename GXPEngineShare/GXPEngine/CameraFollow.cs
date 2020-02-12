using System;
using GXPEngine;

public class CameraFollow : GameObject
{
    #region Variables

    private Player _player;

    #endregion

    public CameraFollow(Player playerScript)
    {
        x = game.width / 2;
        y = game.height / 2;
        _player = playerScript;
    }

    private void Update()
    {
        Translate(_player.x, _player.y);
    }
}
