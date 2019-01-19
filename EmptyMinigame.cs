using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyMinigame : MoveMinigame
{
    public override void Init(Move move, Entity user, Entity target, TextDisplay textDisplay) {
        base.Init(move, user, target, textDisplay);
        FinishGame(1);
    }
}
