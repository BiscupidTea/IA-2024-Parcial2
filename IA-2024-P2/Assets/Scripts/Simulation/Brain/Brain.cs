using System;
using System.Collections.Generic;
using IA_Library_ECS;

namespace IA_Library.Brain
{
    public class Brain
    {
        List<NeuronLayer> layers = new List<NeuronLayer>();
        int totalWeightsCount = 0;
        int inputsCount = 0;

        public float[] outputs;

        private float fitness = 1;
        public float FitnessReward;
        public float FitnessMultiplier;
        int fitnessCount = 0;

        public float bias = 1;
        public float p = 0.5f;

        /// <summary>
        /// Creates a new Neuronal Network
        /// </summary>
        /// <param name="neuronsPerLayer">neurons per layer Example: {3,2,3} 3 entrance, 2 process, 3 output</param>
        /// <param name="bias"></param>
        /// <param name="p"></param>
        public Brain(int[] neuronsPerLayer, float bias, float p)
        {
            this.bias = bias;
            this.p = p;

            for (int i = 0; i < neuronsPerLayer.Length; i++)
            {
                int neuronsCount = neuronsPerLayer[i];
                AddLayer(i == 0 ? neuronsPerLayer[0] : layers[i - 1].OutputsCount, neuronsCount, bias, p);
            }
        }

        public void ApplyFitness()
        {
            fitness *= FitnessReward * FitnessMultiplier > 0 ? FitnessMultiplier : 0;
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

        public float[] GetGenome()
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

        public Layer GetInputLayer()
        {
            int id = layers[0].neurons.Length;
            float[,] weights = new float[layers[0].neurons.Length, layers[0].neurons[0].WeightsCount];
            for (var index = 0; index < layers[0].neurons.Length; index++)
            {
                for (var j = 0; j < layers[0].neurons[index].WeightsCount; j++)
                {
                    weights[index, j] = layers[0].neurons[index].GetWeights()[j];
                }
            }

            Layer layer = new Layer(id, weights);
            return layer;
        }

        public Layer GetOutputLayer()
        {
            Index layerIndex = ^1;
            int id = layers[layerIndex].neurons.Length;
            float[,] weights = new float[layers[layerIndex].neurons.Length, layers[0].neurons[0].WeightsCount];
            for (var index = 0; index < layers[layerIndex].neurons.Length; index++)
            {
                for (var j = 0; j < layers[layerIndex].neurons[index].WeightsCount; j++)
                {
                    weights[index, j] = layers[layerIndex].neurons[index].GetWeights()[j];
                }
            }

            Layer layer = new Layer(id, weights);
            return layer;
        }

        public Layer[] GetHiddenLayers()
        {
            Layer[] layersToReturn = new Layer[layers.Count - 2 > 0 ? layers.Count - 2 : 0];
            var count = 0;
            for (var k = 0; k < this.layers.Count; k++)
            {
                if (k == 0 || k == this.layers.Count - 1)
                {
                    continue;
                }

                int id = layers[k].neurons.Length;
                float[,] weights = new float[layers[k].neurons.Length, layers[k].neurons[0].WeightsCount];
                for (var index = 0; index < layers[k].neurons.Length; index++)
                {
                    for (var j = 0; j < layers[k].neurons[index].WeightsCount; j++)
                    {
                        weights[index, j] = layers[k].neurons[index].GetWeights()[j];
                    }
                }

                layersToReturn[count] = new Layer(id, weights);
                count++;
            }

            return layersToReturn;
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