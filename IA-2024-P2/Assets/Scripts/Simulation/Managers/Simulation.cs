using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using IA_Library_ECS;
using IA_Library_FSM;

namespace IA_Library
{
    public enum HerbivoreStates
    {
        Alive,
        Death,
        Corpse,
    }

    public class Simulation<TypeAgent> where TypeAgent : Agent, new()
    {
        public Vector2 grid;

        public int totalHervivores;
        public int totalCarnivores;
        public int totalScavengers;
        public int totalElite;

        public float mutationChance;
        public float mutationRate;

        public float generationLifeTime;

        private List<AgentHerbivore> Herbivore = new List<AgentHerbivore>();
        private List<AgentCarnivore> Carnivore = new List<AgentCarnivore>();
        private List<AgentScavenger> Scavenger = new List<AgentScavenger>();
        
        private List<Brain.Brain> herbivoreMainBrain = new List<Brain.Brain>();
        private List<Brain.Brain> herbivoreMoveFoodBrain = new List<Brain.Brain>();
        private List<Brain.Brain> herbivoreMoveEscapeBrain = new List<Brain.Brain>();
        private List<Brain.Brain> herbivoreEatBrain = new List<Brain.Brain>();
        
        private List<Brain.Brain> carnivoreMainBrain = new List<Brain.Brain>();
        private List<Brain.Brain> carnivoreMoveBrain = new List<Brain.Brain>();
        private List<Brain.Brain> carnivoreEatBrain = new List<Brain.Brain>();
        
        private List<Brain.Brain> ScavengerMainBrain = new List<Brain.Brain>();
        private List<Brain.Brain> ScavengerFlockingBrain = new List<Brain.Brain>();

        private bool isActive;
        private Dictionary<uint, Brain.Brain> entities;

        public Simulation(Vector2 grid, int totalHervivores, int totalCarnivores, int totalScavengers,
            int totalElite, float mutationChance, float mutationRate, float generationLifeTime)
        {
            this.grid.X = grid.X;
            this.grid.Y = grid.Y;
            
            CreateEntities();
            entities = new Dictionary<uint, Brain.Brain>();
            CreateECSEntities();
        }

        private void CreateEntities()
        {
            for (int i = 0; i < totalHervivores; i++)
            {
                Herbivore.Add(new AgentHerbivore());
                herbivoreMainBrain.Add(Herbivore[i].mainBrain);
                herbivoreMoveFoodBrain.Add(Herbivore[i].moveToFoodBrain);
                herbivoreMoveEscapeBrain.Add(Herbivore[i].moveToEscapeBrain);
                herbivoreEatBrain.Add(Herbivore[i].eatBrain);
            } 
            
            for (int i = 0; i < totalCarnivores; i++)
            {
                Carnivore.Add(new AgentCarnivore());
                carnivoreMainBrain.Add(Carnivore[i].mainBrain);
                carnivoreMoveBrain.Add(Carnivore[i].moveToFoodBrain);
                carnivoreEatBrain.Add(Carnivore[i].eatBrain);
            } 
            
            for (int i = 0; i < totalScavengers; i++)
            {
                Scavenger.Add(new AgentScavenger());
                ScavengerMainBrain.Add(Scavenger[i].mainBrain);
                ScavengerFlockingBrain.Add(Scavenger[i].flockingBrain);
            } 
        }
        
        private void CreateECSEntities()
        {
            
        }

        private void CreateEntity(Brain.Brain brain)
        {
            uint entityID = ECSManager.CreateEntity();
            ECSManager.AddComponent<InputLayerComponent>(entityID, new InputLayerComponent(brain.GetInputLayer()));
            ECSManager.AddComponent<HiddenLayerComponent>(entityID, new HiddenLayerComponent(brain.GetHiddenLayers()));
            ECSManager.AddComponent<OutputLayerComponent>(entityID, new OutputLayerComponent(brain.GetOutputLayer()));
            
            ECSManager.AddComponent<OutputComponent>(entityID, new OutputComponent(brain.GetOutputLayer()));
            ECSManager.AddComponent<InputComponent>(entityID, new InputComponent(brain.GetOutputLayer()));
            
            ECSManager.AddComponent<OutputLayerComponent>(entityID, new OutputLayerComponent(brain.GetOutputLayer()));
            ECSManager.AddComponent<OutputLayerComponent>(entityID, new OutputLayerComponent(brain.GetOutputLayer()));
        }

        public void UpdateSimulation(float deltaTime)
        {
            Herbivore.Update(deltaTime);
            Carnivore.Update(deltaTime);
            Scavenger.Update(deltaTime);
        }

        public void EndSimulation()
        {
        }

        public Dictionary<Vector2, HerbivoreStates> GetHerbivoreAgentsPositionsState()
        {
            Dictionary<Vector2, HerbivoreStates> returnValue = new Dictionary<Vector2, HerbivoreStates>();

            foreach (AgentHerbivore agent in Herbivore.GetAgents())
            {
                returnValue.Add(agent.position, agent.GetState());
            }

            return returnValue;
        }

        public List<Vector2> GetCarnivoreAgentsPositions()
        {
            List<Vector2> returnValue = new List<Vector2>();

            foreach (AgentCarnivore agent in Carnivore.GetAgents())
            {
                returnValue.Add(agent.position);
            }

            return returnValue;
        }

        public List<Vector2> GetScavengerAgentsPositions()
        {
            List<Vector2> returnValue = new List<Vector2>();

            foreach (AgentScavenger agent in Scavenger.GetAgents())
            {
                returnValue.Add(agent.position);
            }

            return returnValue;
        }
    }
}