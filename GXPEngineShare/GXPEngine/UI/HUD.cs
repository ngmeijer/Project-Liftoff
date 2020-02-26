using System;
using System.Drawing;
using GXPEngine;

public class HUD : Canvas
{
	#region

	private readonly Player1 _player1;
	private readonly Player2 _player2;
	private readonly Menu _menu;

	private readonly Brush _whiteBrush;
	private readonly Brush _redBrush;
	private readonly Font _arialFont;

	private readonly float _xPosCounters = 0;
	private readonly float _yPosCounters = 0;

	public Sprite _whipChargeP1;
	public Sprite _whipChargeP2;
	public bool _playerCanSwing { get; private set; }
	public bool _playerCanThrowUp { get; private set; }

	public float WhipChargeP1 { get; private set; }
	public float WhipChargeP2 { get; private set; }

	#endregion

	public HUD(Player1 player1Script, Player2 player2Script, Menu menuScript) : base(1920, 120, false)
	{
		_player1 = player1Script;
		_player2 = player2Script;

		_menu = menuScript;

		_whipChargeP1 = new Sprite("WhipCharge.png");
		AddChild(_whipChargeP1);
		_whipChargeP1.y = 50;
		_whipChargeP1.scaleX = 0;
		_whipChargeP1.scaleY = 0.5f;

		if (_menu.duoPlayers)
		{
			_whipChargeP2 = new Sprite("WhipCharge.png");
			AddChild(_whipChargeP2);
			_whipChargeP2.y = 50;
			_whipChargeP2.x = 1300;
			_whipChargeP2.scaleX = 1;
			_whipChargeP2.scaleY = 0.5f;
		}

		_whiteBrush = Brushes.White;
		_redBrush = Brushes.Red;
		_arialFont = new Font("GameFont.ttf", 30);
	}

	private void Update()
	{
		graphics.Clear(Color.Empty);
		ShowCountersP1();

		if (_player2 != null)
		{
			ShowCountersP2();
			ShowWhipChargeP2();
            DisplayWinner();
		}

		ShowWhipChargeP1();
        RestartGame();
	}

	private void ShowWhipChargeP1()
	{
		if (_player1.pickupsCollected == 0)
		{
			_whipChargeP1.scaleX = 0f;
			WhipChargeP1 = 0f;
		}

		if (_player1.pickupsCollected == 1)
		{
			_whipChargeP1.scaleX = 0.5f;
			WhipChargeP1 = 0.5f;
		}

		if (_player1.pickupsCollected == 2)
		{
			_whipChargeP1.scaleX = 1.0f;
			WhipChargeP1 = 1.0f;
		}

		if((WhipChargeP1 == 1.0f) && (_player1.whipUsedCount < 1))
		{
			_playerCanSwing = false;
			_playerCanThrowUp = true;
		}

		if ((WhipChargeP1 == 0.5f) && (_player1.whipUsedCount < 1))
		{
			_playerCanThrowUp = false;
			_playerCanSwing = true;
		}
	}

	private void ShowWhipChargeP2()
	{
		if (_player2.pickupsCollected == 0)
		{
			_whipChargeP2.scaleX = 0f;
			WhipChargeP2 = 0f;
		}

		if (_player2.pickupsCollected == 1)
		{
			_whipChargeP2.scaleX = 0.5f;
			WhipChargeP2 = 0.5f;
		}

		if (_player2.pickupsCollected == 2)
		{
			_whipChargeP2.scaleX = 1.0f;
			WhipChargeP2 = 1.0f;
		}

		if ((WhipChargeP2 == 1.0f) && (_player2.whipUsedCount < 1))
		{
			_playerCanSwing = true;
		}
	}

	private void ShowCountersP1()
	{
		graphics.DrawString("Score = " + _player1.scoreCount, _arialFont, _whiteBrush, _xPosCounters, _yPosCounters);
		graphics.DrawString("Lives = " + _player1.lifeCount, _arialFont, _whiteBrush, _xPosCounters + 300, _yPosCounters);
	}

	private void ShowCountersP2()
	{
		graphics.DrawString("Score = " + _player2.scoreCount, _arialFont, _redBrush, game.width - 500, _yPosCounters);
		graphics.DrawString("Lives = " + _player2.lifeCount, _arialFont, _redBrush, game.width - 200, _yPosCounters);
	}

    private void DisplayWinner()
    {
        if(_player1.lifeCount <= 0)
        {
            graphics.DrawString("Player 1 WON!", _arialFont, _redBrush, game.width / 2, game.height / 2 - 100);
        }

        if(_player2.lifeCount <= 0)
        {
            graphics.DrawString("Player 2 WON!", _arialFont, _whiteBrush, game.width / 2, game.height / 2 - 100);
        }
    }

    private void RestartGame()
    {
        if((_player1 == null) && (_player2 == null))
        {
            Console.WriteLine("reset game");
        }
    }
}
