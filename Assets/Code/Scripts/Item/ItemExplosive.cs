/// <summary>
/// Special item that explodes upon dropping.
/// </summary>
public class ItemExplosive : Item
{
    private CannonBall cannonBall;

    void Start()
    {
        cannonBall = GetComponent<CannonBall>();
    }

    protected override void OnDrop()
    {
        GameManager.instance.IncrementBombsExploded();
        cannonBall.Explode();
    }
}
