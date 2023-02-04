namespace Script.Decorators.Plants
{
    [Price(20)]
    public class Leaf : Plant
    {
        public override PlantType PlantType => PlantType.LEAF;

        public override void DoAction()
        {
            base.DoAction();
        }
    }
}