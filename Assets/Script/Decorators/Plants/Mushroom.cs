namespace Script.Decorators.Plants
{
    [Price(12)]
    public class Mushroom : Plant
    {
        public override PlantType PlantType => PlantType.MUSHROOM;
    }
}