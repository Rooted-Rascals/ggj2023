namespace Script.Decorators.Plants
{
    [Price(5)]
    public class Leaf : Plant
    {
        public override PlantType PlantType => PlantType.LEAF;
    }
}