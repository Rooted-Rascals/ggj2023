namespace Script.Decorators.Plants
{
    [Price(12)]
    public class Cactus : Plant
    {
        public override PlantType PlantType => PlantType.CACTUS;
    }
}