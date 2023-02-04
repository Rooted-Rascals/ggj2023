namespace Script.Decorators.Biomes
{
    public class Rock : Biome
    {
        public override BiomeType Type => BiomeType.Rock;
        public override bool CanBuildRoots => false;
    }
}
