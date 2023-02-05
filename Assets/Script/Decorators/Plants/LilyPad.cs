namespace Script.Decorators.Plants
{
    [Price(8)]
    public class LilyPad : Plant
    {
        public override PlantType PlantType => PlantType.LILYPAD;
    }
}