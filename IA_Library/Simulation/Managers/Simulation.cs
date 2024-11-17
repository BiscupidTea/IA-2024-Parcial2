﻿using System.Collections.Generic;
using System.Numerics;
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
        public int totalHervivores;
        public int totalCarnivores;
        public int totalScavengers;
        
        private AIManager<AgentHerbivore> Herbivore;
        private AIManager<AgentCarnivore> Carnivore;
        private AIManager<AgentScavenger> Scavenger;

        public void StartSimulation()
        {
            Herbivore = new AIManager<AgentHerbivore>(50, 5, 10, 10, 60);
            Carnivore = new AIManager<AgentCarnivore>(30, 5, 10, 10, 60);
            Scavenger = new AIManager<AgentScavenger>(30, 5, 10, 10, 60);
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