using System;
using System.Drawing;
using GXPEngine;

public class HUD : Canvas
{
	#region

	private readonly Player1 _player1;
	private readonly Player2 _player2;

	private readonly Brush _whiteBrush;
	private readonly Brush _redBrush;
	private readonly Font _arialFont;

	private readonly float _xPosCounters = 0;
	private readonly float _yPosCounters = 50;

	#endregion

	public HUD(Player1 player1Script, Player2 player2Script) : base(1920, 1080, false)
	{
		_player1 = player1Script;
		_player2 = player2Script;

		_whiteBrush = Brushes.White;
		_redBrush = Brushes.Red;
		_arialFont = new Font("Arial", 30);
	}

	private void Update()
	{
		graphics.Clear(Color.Empty);
		ShowCountersP1();
		ShowCountersP2();

        DisplayWinner();
        RestartGame();
	}

	private void ShowCountersP1()
	{
		graphics.DrawString("Score = " + _player1.scoreCount, _arialFont, _whiteBrush, _xPosCounters, _yPosCounters);
		graphics.DrawString("Lives = " + _player1.lifeCount, _arialFont, _whiteBrush, _xPosCounters + 300, _yPosCounters);
	}

	private void ShowCountersP2()
	{
		graphics.DrawString("Score = " + _player2.scoreCount, _arialFont, _redBrush, _xPosCounters + 600, _yPosCounters);
		graphics.DrawString("Lives = " + _player2.lifeCount, _arialFont, _redBrush, _xPosCounters + 900, _yPosCounters);
	}

    private void DisplayWinner()
    {
        if(_player1.lifeCount <= 0)
        {
            graphics.DrawString("Player 1 WON!", _arialFont, _redBrush, game.width / 2, game.height / 2);
        }

        if(_player2.lifeCount <= 0)
        {
            graphics.DrawString("Player 2 WON!", _arialFont, _whiteBrush, game.width / 2, game.height / 2);
        }
    }

    private void RestartGame()
    {

    }
}
