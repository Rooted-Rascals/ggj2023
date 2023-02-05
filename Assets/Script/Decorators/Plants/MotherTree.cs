namespace Script.Decorators.Plants
{
    [Price(999999)]
    public class MotherTree : Plant
    {
        public override PlantType PlantType => PlantType.MOTHERTREE;
    }
}