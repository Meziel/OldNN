using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NN2.NN
{
	public class SequentialNN
	{
		public List<Layer> Layers { get; set; }

		public SequentialNN()
		{
			Layers = new List<Layer>();
		}

		public SequentialNN(SequentialNN other) : this()
		{
			foreach(Layer otherLayer in other.Layers)
			{
				AddLayer(new Layer(otherLayer));
			}
		}

		public void AddLayer(Layer layer)
		{
			Layers.Add(layer);
		}

		public Dictionary<string, float> Predict(Dictionary<string, float> inputs)
		{
			Dictionary<string, float> tempInputs = new Dictionary<string, float>(inputs);

			foreach(Layer l in Layers)
			{
				tempInputs = l.Predict(tempInputs);
			}

			float sum = 0;

			//normalizing outputs to 1
			foreach(KeyValuePair<string, float> output in tempInputs)
			{
				sum += output.Value;
			}

			foreach(KeyValuePair<string, float> output in tempInputs.ToList())
			{
				tempInputs[output.Key] = tempInputs[output.Key] / sum;
			}

			return tempInputs;
		}
	}
}
