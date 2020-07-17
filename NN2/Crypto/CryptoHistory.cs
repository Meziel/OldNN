using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace NN2
{
	public class CryptoHistory
	{
		public string Symbol { get; private set; }
		public int Limit { get; private set; }

		private class Result
		{
			public List<CryptoState> Data { get; set; }
		} 

		public class CryptoState
		{
			public float Time { get; set; }
			public float High { get; set; }
			public float Low { get; set; }
			public float Open { get; set; }
			public float Close { get; set; }
			public float Volumefrom { get; set; }
			public float Volumeto { get; set; }
		} 

		public List<CryptoState> History { get; set; }

		public CryptoHistory(string symbol, int limit)
		{
			Symbol = symbol;
			Limit = limit;

			GetCryptoHistory();
		}

		public void GetCryptoHistory()
		{
			History = new List<CryptoState>();

			string url = String.Format("https://min-api.cryptocompare.com/data/histohour?fsym={0}&tsym=USD&limit={1}&aggregate=1&e=Kraken&extraParams=your_app_name", Symbol, Limit);

			HttpClient client = new HttpClient();
			string response = client.GetStringAsync(url).Result;

			Result message = JsonConvert.DeserializeObject<Result>(response);

			var time = Limit - 1;
			foreach(CryptoState element in message.Data)
			{
				element.Time = time;  
				History.Add(element);
				time--;
			}
		}
	}
}
