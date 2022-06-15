using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidAI : BaseAI
{
    [SerializeField]
    AnimationCurve curve;

    void Update()
    {
        movement.UpdateMovement(new Vector2(curve.Evaluate((Time.time % 3f) / 3f), 0f));
    }
}
