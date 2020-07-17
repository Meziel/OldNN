using NN2.Evolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NN2.StockTrainer
{
	public class StockEnvironment : EvolutionEnvironment
	{
		private static CryptoHistory cryptoHistory = new CryptoHistory("BTC", 1000);
		private List<CryptoHistory.CryptoState> environmentHistory;
		private int inputSize;
		private int stockLimit;
		private int time = 0;

		public StockEnvironment(string stock, int inputSize, int stockLimit, int offset = 0) {
			this.inputSize = inputSize;
			this.stockLimit = stockLimit;
			int size = inputSize + stockLimit - 1; 
			environmentHistory = cryptoHistory.History.GetRange(cryptoHistory.History.Count - size - offset, size);
		}

		public override Dictionary<string, float> GetNextInputs()
		{
			Dictionary<string, float> inputs = new Dictionary<string, float>();
			for(int j=0; j<inputSize; j++)
			{
				CryptoHistory.CryptoState input = environmentHistory[time + j];
				PropertyInfo[] properties = typeof(CryptoHistory.CryptoState).GetProperties();
				foreach (PropertyInfo property in properties)
				{
					string propertyName = property.Name;
					float value = (float)property.GetValue(input);
					string key = string.Format("{0}:{1}", inputSize - j - 1, propertyName);
					inputs.Add(key, value);
				}
			}
			time++;

			return inputs;
		}
	}
}
