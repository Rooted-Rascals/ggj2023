namespace Script.Decorators.Biomes
{
    public class Water : Biome
    {
        public override BiomeType Type => BiomeType.Water;

        public override bool CanBuildLilyPad => hasRoots && !HasAPlant;
        public override bool CanBuildLeaf => false;
    }
}