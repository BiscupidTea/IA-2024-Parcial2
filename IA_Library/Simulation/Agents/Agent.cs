using System.Collections.Generic;
using IA_Library;
using IA_Library_FSM;
using IA_Library.Brain;

namespace IA_Library_FSM
{
    public class Agent
    {
        protected (float, float) position;
        
        public virtual void StartAgent()
        {

        }

        public virtual void Update()
        {
            
        }
    }

    public class Brain
    {
        public NeuralNetwork neuralNetwork;
        public Genome genome;
        private float[] inputs;

        public void SetBrain(Genome genome)
        {
            neuralNetwork = new NeuralNetwork();
            neuralNetwork.SetWeights(genome.genome);
        }

        public virtual void OnThink()
        {
            
        }

        public float[] MakeSynapsis()
        {
           return neuralNetwork.Synapsis(inputs);
        }
    }
}