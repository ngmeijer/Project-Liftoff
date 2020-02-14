﻿using System;
using GXPEngine;

public class PlayerGroundCheck : GameObject
{
    private NormalPlatform _normalPlatform;
    private bool _playerIsMoving;
    private bool playerHasMovedOnPlatform;
    private float _movedDistance;
    private float _offset;
    private FallingPlatform _fallingPlatform;
    private StartPlatform _startPlatform;
    private bool _standingOnStart;

    public PlayerGroundCheck()
    {
        createCollider();
    }

    private void OnCollision(GameObject other)
    {
        if (other is NormalPlatform)
        {
            _normalPlatform = other as NormalPlatform;
            if (!_playerIsMoving)
            {
                x = _normalPlatform.x;
                if (playerHasMovedOnPlatform)
                {
                    x = _normalPlatform.x + _movedDistance;
                }
            }
            y = _normalPlatform.y - _offset;
        }

        if (other is FallingPlatform)
        {
            _fallingPlatform = other as FallingPlatform;
            if (!_playerIsMoving)
            {
                x = _fallingPlatform.x + 50;
                if (playerHasMovedOnPlatform)
                {
                    x = _fallingPlatform.x + _movedDistance;
                }
            }
            y = _fallingPlatform.y - _offset;
        }

        if (other is StartPlatform)
        {
            _startPlatform = other as StartPlatform;
            _standingOnStart = true;
            y = _startPlatform.y - _offset;
        }
    }
}