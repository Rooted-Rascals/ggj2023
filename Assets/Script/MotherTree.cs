namespace Script
{
    public class MotherTree
    {
        private float defaultWaterConsumption = 10f;
        private float defaultEnergyGeneration = 3f;
        private float defaultWaterGeneration = 3f;

        public float GetEnergyGeneration()
        {
            return defaultEnergyGeneration; // + all attached object that generate energy
        }

        public float GetWaterConsumption()
        {
            return defaultWaterConsumption;
        }

        public float GetWaterGeneration()
        {
            return defaultWaterGeneration;
        }
    }
}