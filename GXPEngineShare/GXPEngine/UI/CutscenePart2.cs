using System;
using GXPEngine;

public class CutscenePart2 : AnimationSprite
{
    private Sprite cutscene;

    private int _animationTimer;
    private int _animationSpeed = 100;
    private int frameAmount = 45;

    public bool playingCutscene { get; set; }

    public CutscenePart2() : base("Introscene_spritesheetPart2.png", 9, 5)
    {
        playingCutscene = true;
    }

    private void Update()
    {
        if (playingCutscene)
        {
            playCutscene();
        }
    }

    public void playCutscene()
    {
        _animationTimer += Time.deltaTime;
        int frame = (int)(_animationTimer / _animationSpeed) % frameAmount;

        SetFrame(frame);

        if(frame >= frameAmount)
        {
            SetFrame(0);
        }
    }
}
