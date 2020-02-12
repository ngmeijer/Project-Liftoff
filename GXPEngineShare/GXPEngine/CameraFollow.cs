using System;
using GXPEngine;

public class CameraFollow : GameObject
{
    #region Variables

    private Player1 _player;

    #endregion

    public CameraFollow(Player1 playerScript)
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
