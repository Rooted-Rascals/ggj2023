namespace Script.Decorators.Plants
{
    [Price(15)]
    public class LilyPad : Plant
    {
        public override PlantType PlantType => PlantType.LILYPAD;
    }
}