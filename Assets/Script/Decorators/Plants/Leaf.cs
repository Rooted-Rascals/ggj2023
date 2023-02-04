namespace Script.Decorators.Plants
{
    public class Leaf : Plant
    {
        public override PlantType PlantType => PlantType.LEAF;

        public override void DoAction()
        {
            base.DoAction();
        }
    }
}