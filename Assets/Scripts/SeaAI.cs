using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaAI : BaseAI
{
    [SerializeField]
    AnimationCurve curve;

    void Update()
    {
        movement.UpdatePosition(new Vector2(0f, curve.Evaluate((Time.time % 3f) / 3f)));
    }
}
