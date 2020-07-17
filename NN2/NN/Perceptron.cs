using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NN2.NN
{
	public class Perceptron
	{
		public Dictionary<string, float> Weights { get; set; }
		public float BiasWeight { get; set; }

		private static Random random = new Random();

		public Perceptron()
		{
			Weights = new Dictionary<string, float>();
			BiasWeight = (float)random.NextDouble();
		}

		public Perceptron(Perceptron other)
		{
			Weights = new Dictionary<string, float>(other.Weights);
			BiasWeight = other.BiasWeight;
		}

		public float Predict(Dictionary<string, float> inputs)
		{
			return ComputeWeightedSum(inputs);
		}

		protected float ComputeWeightedSum(Dictionary<string, float> inputs)
		{
			double weightedSum = 0;

			foreach(KeyValuePair<string, float> input in inputs)
			{
				if(!Weights.ContainsKey(input.Key)) {
					Weights.Add(input.Key, (float)random.NextDouble());
				}

				float weight = Weights[input.Key];
				weightedSum += input.Value * weight;
			}

			weightedSum += BiasWeight;

			float sigmoid = (float) (1 / (1 + Math.Pow(Math.E, -weightedSum)));

			return sigmoid;
		}
	}
}
