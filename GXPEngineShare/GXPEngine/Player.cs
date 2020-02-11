using System;
using GXPEngine;
using GXPEngine.Core;

public class Player : Sprite
{
    #region Variables

    private float _moveSpeed = 5f;
    private float _jumpForce = 20f;
    private float _fallMultiplier = 7f;
    private bool _isJumping = false;

    #endregion

    #region Constructor & Update

    public Player() : base("PlayerSprite.png")
	{
        x = game.width / 2;
        y = 100;
	}

    private void Update()
    {
        MovePlayer();
        PlayerJump();
    }

    #endregion

    #region Functions

    private void MovePlayer()
    {
        //These input conditions are temporary, of course! Will be replaced by the actual controller.
        if (Input.GetKey(Key.A))
        {
            Translate(-_moveSpeed, 0);
        }

        if (Input.GetKey(Key.D))
        {
            Translate(_moveSpeed, 0);
        }
    }

    private void PlayerJump()
    {
        float tempPosY = 0;

        if (Input.GetKeyDown(Key.SPACE))
        {
            _isJumping = true;
            tempPosY += _jumpForce;
            Translate(0, _jumpForce);

        }

        if (Input.GetKeyUp(Key.SPACE))
        {
            _isJumping = false;
        }

        if (!_isJumping)
        {
            Translate(0, _fallMultiplier);
        }
    }

    private void OnCollision(Collider hitInfo)
    {
        if(hitInfo == Platform)
        {

        }
    }

    #endregion
}
