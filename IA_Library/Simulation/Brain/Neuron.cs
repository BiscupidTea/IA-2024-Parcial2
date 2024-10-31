using System;

namespace IA_Library.Brain
{
    public class Neuron 
    {
        private float[] weights;
        private float bias;
        private float p;
        private Func<float, float> activationFunc;

        public int WeightsCount => weights.Length;

        public Neuron(int weightsCount, float bias, float p, Func<float, float> activationFunc)
        {
            weights = new float[weightsCount];
            Random rand = new Random();
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = (float)(rand.NextDouble() * 2 - 1); // RandVal (-1,1)
            }

            this.bias = bias;
            this.p = p;
            this.activationFunc = activationFunc ?? Sigmoid;
        }

        public float Synapsis(float[] inputs)
        {
            float sum = 0;

            for (int i = 0; i < inputs.Length; i++)
            {
                sum += weights[i] * inputs[i];
            }
            
            sum += bias * weights[weights.Length - 1];

            return activationFunc(sum);
        }

        public int SetWeights(float[] newWeights, int fromId)
        {
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = newWeights[fromId + i];
            }

            return fromId + weights.Length;
        }

        public float[] GetWeights()
        {
            float[] copy = new float[weights.Length];
            for (int i = 0; i < weights.Length; i++)
            {
                copy[i] = weights[i];
            }
            return copy;
        }
        
        private float Sigmoid(float a)
        {
            return 1.0f / (1.0f + MathF.Exp(-a / p));
        }
    }
}