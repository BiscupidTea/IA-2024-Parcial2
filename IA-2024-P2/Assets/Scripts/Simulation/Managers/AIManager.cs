using System.Collections.Generic;
using IA_Library_FSM;

namespace IA_Library
{
    public class AIManager<TypeAgent> where TypeAgent : Agent, new()
    {
        private GeneticAlgorithm geneticAlgorithmManager;
        private List<TypeAgent> agents;

        private float generationLifeTime = 0;
        private float timerGenerationLifeTime = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentType"></param>
        /// <param name="totalPopulation"></param>
        /// <param name="totalElites"></param>
        /// <param name="mutationChance">(0.0/1.0)</param>
        /// <param name="mutationRate"></param>
        public AIManager(
            GridManager grid, int totalPopulation, int totalElites,
            float mutationChance, float mutationRate, float generationLifeTime)
        {
            for (int i = 0; i < totalPopulation; i++)
            {
                agents.Add(new TypeAgent());
            }

            this.generationLifeTime = generationLifeTime;
            geneticAlgorithmManager = new GeneticAlgorithm(totalElites, mutationChance, mutationRate);
        }

        public void Update()
        {
            if (timerGenerationLifeTime >= generationLifeTime)
            {
                CreateNextGeneration();
            }
            
            foreach (TypeAgent currentAgent in agents)
            {
                currentAgent.Update();
            }
        }

        private void CreateNextGeneration()
        {
            // List<Genome> genomes = new List<Genome>();
            //
            // foreach (var agent in agents)
            // {
            //     genomes.Add(agent.);
            // }
            //
            // geneticAlgorithmManager.Epoch();
        }
    }
}