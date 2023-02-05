namespace Script.Decorators.Biomes
{
    public class Sand : Biome
    {
        public override BiomeType Type => BiomeType.Sand;
        public override bool CanBuildCactus => hasRoots && !HasAPlant;
        public override bool CanBuildLeaf => false;
    }
}
