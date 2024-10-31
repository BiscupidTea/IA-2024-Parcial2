using System;

namespace IA_Library.Brain
{
    public class NeuronLayer
    {
        private Neuron[] neurons;
        private float[] outputs;
        private int inputsCount;
        private float bias;
        private float p;

        public int NeuronsCount => neurons.Length;
        public int InputsCount => inputsCount;
        public int OutputsCount => outputs.Length;
        public int TotalWeights => neurons.Length * (inputsCount + 1);

        public NeuronLayer(int inputsCount, int neuronsCount, float bias, float p)
        {
            this.inputsCount = inputsCount;
            this.bias = bias;
            this.p = p;

            neurons = new Neuron[neuronsCount];
            for (int i = 0; i < neurons.Length; i++)
            {
                neurons[i] = new Neuron(inputsCount + 1, bias, p, Sigmoid);
            }

            outputs = new float[neurons.Length];
        }

        public int SetWeights(float[] weights, int fromId)
        {
            for (int i = 0; i < neurons.Length; i++)
            {
                fromId = neurons[i].SetWeights(weights, fromId);
            }

            return fromId;
        }

        public float[] GetWeights()
        {
            float[] weights = new float[TotalWeights];
            int id = 0;

            for (int i = 0; i < neurons.Length; i++)
            {
                float[] neuronWeights = neurons[i].GetWeights();
                for (int j = 0; j < neuronWeights.Length; j++)
                {
                    weights[id] = neuronWeights[j];
                    id++;
                }
            }

            return weights;
        }

        public float[] Synapsis(float[] inputs)
        {
            for (int j = 0; j < neurons.Length; j++)
            {
                outputs[j] = neurons[j].Synapsis(inputs);
            }

            return outputs;
        }

        private float Sigmoid(float a)
        {
            return 1.0f / (1.0f + MathF.Exp(-a / p));
        }
    }
}