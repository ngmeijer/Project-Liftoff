using System;
using GXPEngine;

public class SceneManager : GameObject
{
	Menu menu;
	Cutscene cutscene;
	Level level;

	public SceneManager(Menu menuScript, Cutscene cutsceneScript)
	{
		menu = new Menu(level, cutscene);
		AddChild(menu);
	}

	private void Update()
	{
		CheckCutsceneStart();
		CheckGameStart();
	}

	private void CheckCutsceneStart()
	{
		if (menu.startCutscene)
		{
			cutscene = new Cutscene();
			AddChild(cutscene);
			menu.startCutscene = false;
			
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
			}
		}
	}
}
