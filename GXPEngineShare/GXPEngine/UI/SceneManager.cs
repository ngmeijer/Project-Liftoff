using System;
using GXPEngine;

public class SceneManager : GameObject
{
	public Menu menu { get; private set; }
	public Cutscene cutscene { get; private set; }

	Level level;

	public SceneManager(Menu menuScript, Cutscene cutsceneScript)
	{
		menu = new Menu(this, cutscene);
		AddChild(menu);
	}

	private void Update()
	{
		CheckCutsceneStart();
		CheckGameStart();
		CheckGameReset();
	}

	private void CheckCutsceneStart()
	{
		if (menu.startCutscene)
		{
			cutscene = new Cutscene(menu);
			AddChild(cutscene);
			menu.startCutscene = false;
			RemoveChild(menu);
		}
	}

	private void CheckGameStart()
	{
		if (cutscene != null)
		{
			if (cutscene.continueToGame)
			{
				level = new Level(menu);
				AddChild(level);
				RemoveChild(cutscene);
				cutscene.continueToGame = false;
			}
		}
	}

	private void CheckGameReset()
	{
		if(level != null)
		{
			if (level.hud.backToMainMenu)
			{
				menu = new Menu(this, cutscene);
				AddChild(menu);
				RemoveChild(level);
				level.hud.backToMainMenu = false;
			}
		}
	}
}
