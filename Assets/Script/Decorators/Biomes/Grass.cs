namespace Script.Decorators.Biomes
{
    public class Grass : Biome
    {
        public override BiomeType Type => BiomeType.Grass;
        public override bool CanBuildMushroom => hasRoots && !HasAPlant;
    }
}