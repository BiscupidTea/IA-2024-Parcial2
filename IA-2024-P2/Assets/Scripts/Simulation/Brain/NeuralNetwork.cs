using System.Collections.Generic;

namespace IA_Library.Brain
{
    public class NeuralNetwork
    {
        private List<NeuronLayer> layers = new List<NeuronLayer>();
        private int totalWeightsCount = 0;

        /// <summary>
        /// Creates a new Neuronal Network
        /// </summary>
        /// <param name="neuronsPerLayer">neurons per layer Example: {3,2,3} 3 entrance, 2 process, 3 output</param>
        /// <param name="bias"></param>
        /// <param name="p"></param>
        public NeuralNetwork(int[] neuronsPerLayer, float bias, float p)
        {
            for (int i = 0; i < neuronsPerLayer.Length; i++)
            {
                int neuronsCount = neuronsPerLayer[i];
                AddLayer(i == 0 ? neuronsPerLayer[0] : layers[i - 1].OutputsCount, neuronsCount, bias, p);
            }
        }

        private void AddLayer(int inputsCount, int neuronsCount, float bias, float p)
        {
            NeuronLayer layer = new NeuronLayer(inputsCount, neuronsCount, bias, p);
            layers.Add(layer);
            totalWeightsCount += (inputsCount + 1) * neuronsCount;
        }

        public void SetWeights(float[] newWeights)
        {
            int fromId = 0;

            for (int i = 0; i < layers.Count; i++)
            {
                fromId = layers[i].SetWeights(newWeights, fromId);
            }
        }

        public float[] GetWeights()
        {
            float[] weights = new float[totalWeightsCount];
            int id = 0;

            for (int i = 0; i < layers.Count; i++)
            {
                float[] ws = layers[i].GetWeights();

                for (int j = 0; j < ws.Length; j++)
                {
                    weights[id++] = ws[j];
                }
            }

            return weights;
        }

        public float[] Synapsis(float[] inputs)
        {
            float[] outputs = inputs;

            for (int i = 0; i < layers.Count; i++)
            {
                outputs = layers[i].Synapsis(outputs);
            }

            return outputs;
        }
    }
}