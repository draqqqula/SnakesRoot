namespace SnakeGame.Models.Gameplay;

internal class DoubledSlime : Slime
{
    public override int Value => base.Value * 2;
    public override float DestinedSize => base.DestinedSize + 1;
    public override byte GroupId => 1;
    public override void UpdateAsset()
    {
        Transform.ChangeAsset($"slime{Tier}_doubled");
    }
}
