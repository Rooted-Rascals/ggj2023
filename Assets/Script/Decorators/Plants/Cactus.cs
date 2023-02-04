namespace Script.Decorators.Plants
{
    [Price(50)]
    public class Cactus : Plant
    {
        public override PlantType PlantType => PlantType.CACTUS;
    }
}