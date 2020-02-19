using System;
using GXPEngine;

public class Cutscene : AnimationSprite
{
    private Sprite cutscene;

    private int _animationTimer;
    private int _animationSpeed = 50;

    public Cutscene() : base("Cutscene.gif", 1, 1, 1, true, false)
    {
        
    }

    private void Update()
    {
        playCutscene();
    }

    public void playCutscene()
    {
        _animationTimer += Time.deltaTime;
        int frame = (int)(_animationTimer / _animationSpeed) % 100 + 2;

        SetFrame(frame);
    }
}
