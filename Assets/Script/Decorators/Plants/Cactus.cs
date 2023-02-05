using System;

namespace Script.Decorators.Plants
{
    [Buyable(12, nameof(Cactus), "Cactus can throw spikes at enemies at a distance.")]
    public class Cactus : Plant
    {
        public override PlantType PlantType => PlantType.CACTUS;
        private TurretAI turretAI;

        public void Start()
        {
            base.Start();
            if (turretAI == null)
            {
                turretAI = GetComponent<TurretAI>();
            }
        }

        public override void DoAction()
        {
            if (!IsReady)
            {
                return;
            }
            if (turretAI == null)
            {
                return;
            }
            turretAI.ActionUpdate();
        }
    }
}