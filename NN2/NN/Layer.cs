using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NN2.NN
{
	public class Layer
	{
		public List<Perceptron> Perceptrons { get; set; }

		public Layer(int perceptronsInLayer = 0)
		{
			Perceptrons = new List<Perceptron>();

			for (int i = 0; i < perceptronsInLayer; i++)
			{
				Perceptron perceptron = new Perceptron();
				Perceptrons.Add(perceptron);
			}
		}

		public Layer(Layer other)
		{
			Perceptrons = new List<Perceptron>();

			foreach(Perceptron otherPerceptron in other.Perceptrons)
			{
				Perceptrons.Add(new Perceptron(otherPerceptron));
			}
		}

		public Dictionary<string, float> Predict(Dictionary<string, float> inputs)
		{
			Dictionary<string, float> outputs = new Dictionary<string, float>();
			for (int i = 0; i < Perceptrons.Count; i++)
			{
				outputs[i.ToString()] = Perceptrons[i].Predict(inputs);
			}

			return outputs;
		}
	}
}
