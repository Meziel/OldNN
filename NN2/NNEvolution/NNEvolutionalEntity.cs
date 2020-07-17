using NN2.Evolution;
using NN2.NN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NN2.NNEvolution
{
	public class NNEvolutionalEntity<A> : EvolutionalEntity<A> where A : Enum
	{
		private static Random random = new Random();

		public SequentialNN SequentialNN { get; set; }
		public float LearningTolerance { get; set; }

		public NNEvolutionalEntity(SequentialNN sequentialNN)
		{
			SequentialNN = sequentialNN;
			LearningTolerance = 1;
		}

		public override EvolutionalEntity<A> CreateOffspring()
		{
			SequentialNN childSequentialNN = new SequentialNN(SequentialNN);
			//mutate child
			foreach (Layer layer in childSequentialNN.Layers)
			{
				foreach (Perceptron perceptron in layer.Perceptrons)
				{
					float randomModifier;

					foreach (KeyValuePair<string, float> weight in perceptron.Weights.ToList())
					{
						randomModifier = (float)(random.NextDouble() * 2 - 1) * LearningTolerance;
						perceptron.Weights[weight.Key] = weight.Value + randomModifier;
					}
					randomModifier = (float)(random.NextDouble() * 2 - 1) * LearningTolerance;
					perceptron.BiasWeight += randomModifier;
				}
			}

			//float learningTolerance = LearningTolerance - 0.01f;
			//if (learningTolerance < 0)
			//{
			//	learningTolerance = 0;
			//}

			//return new NNEvolutionalEntity<A>(childSequentialNN) { LearningTolerance = learningTolerance };
			return new NNEvolutionalEntity<A>(childSequentialNN);
		}

		public override HashSet<A> PerformActions(Dictionary<string, float> inputs) {
			Dictionary<string, float> outputs = SequentialNN.Predict(inputs);

			KeyValuePair<string, float>? maxOuput = null;

			foreach(KeyValuePair<string, float> ouput in outputs)
			{
				if(maxOuput == null)
				{
					maxOuput = ouput;
				} else if(ouput.Value > maxOuput.Value.Value)
				{
					maxOuput = ouput;
				}
			}

			A action = (A)Enum.Parse(typeof(A), maxOuput.Value.Key);
			HashSet<A> actions = new HashSet<A>();
			actions.Add(action);

			return actions;
		}
	}
}
