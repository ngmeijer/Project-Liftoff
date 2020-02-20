using System;
using GXPEngine;

public class CutscenePart1 : AnimationSprite
{
    private Sprite cutscene;

    private int _animationTimer;
    private int _animationSpeed = 100;
    private int frameAmount = 36;

    public bool playingCutscene { get; set; }
    public bool cutsceneFinished { get; set; }

    public CutscenePart1() : base("Introscene_spritesheetPart1.png", 9, 4)
    {
        
    }

    private void Update()
    {
        playCutscene();
    }

    public void playCutscene()
    {
        _animationTimer += Time.deltaTime;
        int frame = (int)(_animationTimer / _animationSpeed) % frameAmount;

        SetFrame(frame);

        if(frame >= frameAmount - 1)
        {
            cutsceneFinished = true;
        }
    }
}
