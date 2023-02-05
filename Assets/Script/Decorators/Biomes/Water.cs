namespace Script.Decorators.Biomes
{
    public class Water : Biome
    {
        public override BiomeType Type => BiomeType.Water;

        public override bool CanBuildLilyPad => hasRoots && !HasAPlant && !gameObject.GetComponentInParent<Tile>().IsNotConnected.activeSelf;
        public override bool CanBuildLeaf => false;
    }
}