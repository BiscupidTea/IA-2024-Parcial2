using System.Collections.Generic;
using IA_Library_FSM;

namespace IA_Library
{
    public class IaManager
    {
        private GeneticAlgorithm geneticAlgorithmManager;
        private List<Agent> agents;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalPopulation"></param>
        /// <param name="totalElites"></param>
        /// <param name="mutationChance">(0.0/1.0)</param>
        /// <param name="mutationRate"></param>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        /// <param name="hiddenLayers"></param>
        /// <param name="bias"></param>
        /// <param name="p"></param>
        public IaManager(
            int totalPopulation, int totalElites,
            float mutationChance, float mutationRate,
            int inputs, int outputs, Dictionary<int, int> hiddenLayers,
            float bias, float p)
        {
        }

        public void Update()
        {
            foreach (Agent currentAgent in agents)
            {
                currentAgent.Update();
            }
        }
    }
}