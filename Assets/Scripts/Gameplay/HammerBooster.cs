using UnityEngine;

public class HammerBooster : BoosterBase
{
    public override void Activate()
    {
        BoosterManager.Instance.ActivateHammer();
    }
}