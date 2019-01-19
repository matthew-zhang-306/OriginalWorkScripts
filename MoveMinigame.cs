using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMinigame : MonoBehaviour
{
    public string instructions;

    protected Move move;
    protected TextDisplay textDisplay;
    protected Entity user;
    protected Entity target;

    protected bool hasInited;

    public virtual void Init(Move _move, Entity _user, Entity _target, TextDisplay _textDisplay) {
        move = _move;
        textDisplay = _textDisplay;
        user = _user;
        target = _target;

        textDisplay.UpdateText(instructions);

        hasInited = true;
    }

    protected void FinishGame(float multiplier) {
        move.UseMoveOn(user, target, multiplier, textDisplay);
        Destroy(gameObject);
    }
}

[System.Serializable]
public struct PerformanceZone {
    // these metrics can represent whatever is needed for the performance judge
    public float metric1;
    public float metric2;

    public float multiplier;

    public PerformanceZone(float mult, float m1, float m2) {
        multiplier = mult;
        metric1 = m1;
        metric2 = m2;
    }
    public PerformanceZone(float mult, float m1) : this(mult, m1, 0f) {}
    public PerformanceZone(float mult)           : this(mult, 0f, 0f) {}
}
