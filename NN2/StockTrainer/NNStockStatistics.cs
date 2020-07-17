using NN2.Evolution;
using NN2.NN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NN2.StockTrainer
{
	public class NNStockStatistics
	{
		public EvolutionalEntity<Action> EvolutionalEntity { get; set; }
		public float Performance { get; set; }
		public int Generation { get; set; }
	}
}
