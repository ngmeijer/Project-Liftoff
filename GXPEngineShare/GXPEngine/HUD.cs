using System;
using System.Drawing;
using GXPEngine;

public class HUD : Canvas
{
	#region

	private readonly Player _player;

	private readonly Brush _whiteBrush;
	private readonly Font _arialFont;

	private readonly float _xPosCounters = 0;
	private readonly float _yPosCounters = 50;

	#endregion

	public HUD(Player playerScript) : base(1920, 1080, false)
	{
		_player = playerScript;

		_whiteBrush = Brushes.White;
		_arialFont = new Font("Arial", 40);
	}

	private void Update()
	{
		ShowScoreLife();
	}

	private void ShowScoreLife()
	{
		graphics.Clear(Color.Empty);
		graphics.DrawString("Score = " + _player.GetScoreCount(), _arialFont, _whiteBrush, _xPosCounters, _yPosCounters);
		graphics.DrawString("Lives = " + _player.GetLifeCount(), _arialFont, _whiteBrush, _xPosCounters + 300, _yPosCounters);
	}
}
