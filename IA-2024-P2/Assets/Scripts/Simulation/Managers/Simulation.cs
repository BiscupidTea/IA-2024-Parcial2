using IA_Library_FSM;

namespace IA_Library
{
    public class Simulation
    {
        private AIManager<AgentHerbivore> Herbivore;
        private AIManager<AgentCarnivore> Carnivore;
        private AIManager<AgentScavenger> Scavenger;

        private GridManager grid;

        public void StartSimulation()
        {
            grid = new GridManager(500, 500);

            Herbivore = new AIManager<AgentHerbivore>(grid, 50, 5, 10, 10, 60);
            Carnivore = new AIManager<AgentCarnivore>(grid, 30, 5, 10, 10, 60);
            Scavenger = new AIManager<AgentScavenger>(grid, 30, 5, 10, 10, 60);
        }

        public void UpdateSimulation(float deltaTime)
        {
            Herbivore.Update();
            Carnivore.Update();
            Scavenger.Update();
        }

        public void EndSimulation()
        {
        }

        public void GetAgents()
        {
        }

        public void ReturnLogs()
        {
        }
    }
}