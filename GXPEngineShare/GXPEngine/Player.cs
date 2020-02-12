using System;
using GXPEngine;
using GXPEngine.Core;

public class Player : Sprite
{
    #region Variables

    private float _moveSpeed = 5f;
    private float _jumpForce = 8f;
    private float _fallMultiplier = 9.81f;
    private bool _isJumping = false;

    private Platform _platform;
    private bool _standingOnPlatform;
    private bool _stillStandingOnPlatform;
    private int _offset = 64;

    private CameraFollow _cameraFollow;
    private GroundCollider _groundCollider;

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
        CheckForScreenCollision();
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
        if (Input.GetKeyDown(Key.SPACE))
        {
            _isJumping = true;
        }

        //By default, the player will fall down, due to "gravity" lol
        if (!_isJumping)
        {
            Translate(0, _fallMultiplier);
        }

        //If he is jumping, a certain amount of force is added to the player, decreasing his Y-position (therefore jumping up, in this case)
        if (_isJumping)
        {
            Translate(0, -_jumpForce);

            if (Input.GetKeyUp(Key.SPACE))
            {
                _isJumping = false;
            }
        }
    }

    private void CheckForScreenCollision()
    {
        if(x >= game.width)
        {
            Console.WriteLine("player dies");
        }

        if (x <= 0)
        {
            Console.WriteLine("player dies");
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

        if(hitInfo is GroundCollider)
        {
            _groundCollider = hitInfo as GroundCollider;
            y = _groundCollider.y - _offset;
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
