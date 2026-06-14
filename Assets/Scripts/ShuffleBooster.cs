using UnityEngine;

public class ShuffleBooster : BoosterBase
{
    public override void Activate()
    {
        BoosterManager.Instance.UseShuffle();
    }
}