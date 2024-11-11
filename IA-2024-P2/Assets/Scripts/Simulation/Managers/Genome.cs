using System;

namespace IA_Library
{
    public class Genome
    {
        public float[] genome;
        public float fitness = 0;

        public Genome(float[] genes)
        {
            this.genome = genes;
            fitness = 0;
        }

        public Genome(int genesCount)
        {
            genome = new float[genesCount];

            Random rand = new Random();
            
            for (int j = 0; j < genesCount; j++)
                genome[j] = (float)(rand.NextDouble() * 2 - 1);

            fitness = 0;
        }
        
        public Genome()
        {
            fitness = 0;
        }
    }
}