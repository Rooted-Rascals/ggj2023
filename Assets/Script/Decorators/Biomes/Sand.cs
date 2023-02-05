namespace Script.Decorators.Biomes
{
    public class Sand : Biome
    {
        public override BiomeType Type => BiomeType.Sand;
        public override bool CanBuildCactus => hasRoots && !HasAPlant && !gameObject.GetComponentInParent<Tile>().IsNotConnected.activeSelf;
        public override bool CanBuildLeaf => false;
    }
}
