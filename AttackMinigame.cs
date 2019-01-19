using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMinigame : MoveMinigame
{
    public List<PerformanceZone> zones;
    public List<Color> zoneColors;

    public float speed;

    Transform bar;
    Transform indicator;
    float leftOfBar;

    bool hasPressed;

    public override void Init(Move move, Entity user, Entity target, TextDisplay textDisplay) {
        base.Init(move, user, target, textDisplay);
    
        bar = transform.GetChild(0);
        leftOfBar = bar.localPosition.x - bar.localScale.x / 2;
        BarLength = 0;

        indicator = transform.GetChild(1);
        PositionIndicator();
    }

    // For some reason this process is long and complicated, so it's in a separate method now.
    void PositionIndicator() {
        // Find the index of the optimal zone
        var multList = zones.Select(z => z.multiplier).ToList();
        var bestIndex = multList.IndexOf(multList.Max());

        // Grab the upper and lower bounds of the range
        float indicatorBoundUpper = zones[bestIndex + 1].metric1;
        float indicatorBoundLower = zones[bestIndex    ].metric1;

        // Position the indicator to span between the bounds
        indicator.localPosition = new Vector3((indicatorBoundUpper + indicatorBoundLower) / 4, 0, 0);
        indicator.localScale = new Vector3(indicatorBoundUpper - indicatorBoundLower, indicator.localScale.y, 1);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
            hasPressed = true;
        if (hasPressed && Input.GetKeyUp(KeyCode.Space)) {
            // Finish the game with the current performance zone
            FinishGame(zones[GetZoneId()].multiplier);
        }
    }

    void FixedUpdate() {
        if (hasPressed) {
            BarLength += speed * Time.deltaTime;
            Debug.Log(BarLength);

            // If the bar has reached the final zone, force quit with a miss
            if (GetZoneId() == zones.Count - 1) 
                FinishGame(0);
        }
    }

    float BarLength {
        get { return bar.localScale.x; }
        set {
            bar.localPosition = new Vector3(leftOfBar + value / 2, 0, 0);
            bar.localScale = new Vector3(value, bar.localScale.y, bar.localScale.z);
            bar.GetComponent<SpriteRenderer>().color = zoneColors[GetZoneId()];
        }
    }
    int GetZoneId() {
        Debug.Log(zones.FindLastIndex(z => z.metric1 <= BarLength));
        return zones.FindLastIndex(z => z.metric1 <= BarLength);
    }
}
