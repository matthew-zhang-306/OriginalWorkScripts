using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMinigame : MoveMinigame
{
    public List<float> damageMultipliers;
    List<PerformanceZone> zones;
    float amplitude { get { return zones[0].metric1; }}

    public float period;
    float timer;

    Transform indicator;

    public override void Init(Move move, Entity user, Entity target, TextDisplay textDisplay) {
        base.Init(move, user, target, textDisplay);

        zones = new List<PerformanceZone>();
        foreach (Transform t in transform)
            if (indicator == null) {
                indicator = t;
            }
            else {
                zones.Add(new PerformanceZone(damageMultipliers[0], t.localScale.x / 2));
                damageMultipliers.RemoveAt(0);
            }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            // Finish the game with the current performance zone
            FinishGame(zones[GetZoneId()].multiplier);
        }
    }

    void FixedUpdate() {
        timer += Time.deltaTime;

        Vector3 newPos = indicator.localPosition;
        newPos.x = (-4 * amplitude / period) * Mathf.Abs((timer % period) - period / 2) + amplitude;
        indicator.localPosition = newPos;
    }

    int GetZoneId() {
        Debug.Log(zones.FindLastIndex(z => z.metric1 >= Mathf.Abs(indicator.localPosition.x)));
        return zones.FindLastIndex(z => z.metric1 >= Mathf.Abs(indicator.localPosition.x));
    }
}
