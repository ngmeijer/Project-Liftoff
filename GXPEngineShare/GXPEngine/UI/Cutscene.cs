using System;
using GXPEngine;

public class Cutscene : GameObject
{
    private int _animationTimer1;
    private int _animationTimer2;
    private int _animationTimer3;

    private int _animationSpeed = 100;
    private int frameAmountP1 = 27;
    private int frameAmountP2 = 27;
    private int frameAmountP3 = 27;

    public bool playingCutscene { get; private set; }

    public bool part1Finished { get; set; }
    public bool part2Finished { get; set; }
    public bool part3Finished { get; set; }

    private AnimationSprite part1;
    private AnimationSprite part2;
    private AnimationSprite part3;
    public bool continueToGame { get; set; }

    public Cutscene()
    {
        playingCutscene = true;

        part1 = new AnimationSprite("CutsceneP1.png", 9, 3);
        AddChild(part1);

        part2 = new AnimationSprite("CutsceneP2.png", 9, 3);
        AddChild(part2);
        part2.visible = false;

        part3 = new AnimationSprite("CutsceneP3.png", 9, 3);
        AddChild(part3);
        part3.visible = false;
    }

    private void Update()
    {
        partTransition();
    }

    private void partTransition()
    {
        if (Input.GetKeyDown(Key.SPACE))
        {
            continueToGame = true;
        }

        if (!part1Finished)
        {
            playPart1();
        }

        if (part1Finished)
        {
            playPart2();
        }

        if (part2Finished)
        {
            playPart3();
        }
    }

    public void playPart1()
    {
        _animationTimer1 += Time.deltaTime;
        int frame = _animationTimer1 / _animationSpeed % frameAmountP1;

        part1.SetFrame(frame);

        if (frame >= frameAmountP1 - 1)
        {
            part1Finished = true;
            part1.visible = false;
        }
    }

    private void playPart2()
    {
        part1.visible = false;
        part2.visible = true;
        _animationTimer2 += Time.deltaTime;
        int frame = _animationTimer2 / _animationSpeed % frameAmountP2;

        part2.SetFrame(frame);

        if (frame >= frameAmountP2 - 1)
        {
            part2Finished = true;
        }
    }

    private void playPart3()
    {
        part2.visible = false;
        part3.visible = true;
        _animationTimer3 += Time.deltaTime;
        int frame = (int)(_animationTimer3 / _animationSpeed) % frameAmountP3;

        part3.SetFrame(frame);

        if (frame >= frameAmountP3 - 1)
        {
            continueToGame = true;
            LateRemove();
        }
    }
}
