namespace Script.Decorators.Plants
{
    [Price(6)]
    public class Mushroom : Plant
    {
        public override PlantType PlantType => PlantType.MUSHROOM;
    }
}