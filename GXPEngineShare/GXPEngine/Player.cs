using System;
using GXPEngine;
using GXPEngine.Core;

public class Player : Sprite
{
    #region Variables

    private float _moveSpeed = 5f;
    private float _jumpForce = 20f;
    private float _fallMultiplier = 2f;
    private bool _isJumping = false;

    private Platform _platform;
    private bool _standingOnPlatform;
    private bool _stillStandingOnPlatform;
    private int _offset = 64;

    private CameraFollow _cameraFollow;

    #endregion

    #region Constructor & Update

    public Player() : base("PlayerSprite.png")
	{
        x = 500;
        y = 100;

        _cameraFollow = new CameraFollow(this);
        AddChild(_cameraFollow);
	}

    private void Update()
    {
        MovePlayer();
        PlayerJump();
        CheckForPlatformCollision();
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

        if (Input.GetKeyDown(Key.SPACE) && _standingOnPlatform)
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

    private void OnCollision(GameObject hitInfo)
    {
        if(hitInfo is Platform)
        {
            _platform = hitInfo as Platform;
            _standingOnPlatform = true;
            y = _platform.y - _offset;
        }
    }

    private void CheckForPlatformCollision()
    {
        if (_standingOnPlatform)
        {
            _stillStandingOnPlatform = HitTest(_platform);

            if (_stillStandingOnPlatform)
            {
                y = _platform.y;
            }
            else if (!_stillStandingOnPlatform)
            {
                _standingOnPlatform = false;
            }
        }
    }

    #endregion
}
