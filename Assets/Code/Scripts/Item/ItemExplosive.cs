using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemExplosive : Item
{
    private CannonBall cannonBall;

    void Start()
    {
        cannonBall = GetComponent<CannonBall>();
    }

    protected override void OnDrop()
    {
        cannonBall.Explode();
    }
}
