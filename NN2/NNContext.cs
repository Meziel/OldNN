using NN2.NN;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NN2
{
	public class NNContext : DbContext
	{
		DbSet<SequentialNN> NeuralNetworks { get; set; }
		DbSet<Layer> Layers { get; set; }
		DbSet<Perceptron> Perceptrons { get; set; }
	}
}
