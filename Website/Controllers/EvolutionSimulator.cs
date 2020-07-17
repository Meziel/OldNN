using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NN2.Evolution;
using NN2.NN;
using NN2.NNEvolution;
using NN2.StockTrainer;
using Action = NN2.StockTrainer.Action;

namespace Website.Controllers
{
	[RoutePrefix("api/evolutionsimulator")]
	public class EvolutionSimulatorController : ApiController
	{
		public struct TrainParameters {
			public int ticksTrain;
			public int ticksTest;
			public int inputSize;
			public int generations;
			public string stockName;
		}

		public struct TrainReturn
		{
			public List<NNStockStatistics> trainStatistics;
			public List<NNStockStatistics> testStatistics;

			public List<NNStockStatistics> trainAverage;
			public List<NNStockStatistics> testAverage;
		}

		public float GetAverage(StockNNES stockNNES)
		{
			float sum = 0;
			foreach (var performance in stockNNES.Performances)
			{
				sum += performance.Value;
			}

			return sum / stockNNES.Performances.Count;
		}

		[Route("train")]
		public TrainReturn Train(TrainParameters trainParameters)
		{
			TrainReturn trainReturn = new TrainReturn();
			List<NNStockStatistics> trainStatistics = new List<NNStockStatistics>();
			List<NNStockStatistics> testStatistics = new List<NNStockStatistics>();

			List<NNStockStatistics> trainAverage = new List<NNStockStatistics>();
			List<NNStockStatistics> testAverage = new List<NNStockStatistics>();

			SequentialNN model = new SequentialNN();
			model.AddLayer(new Layer(5));
			model.AddLayer(new Layer(5));

			StockNNES stockNNES = new StockNNES(model, 100);

			for (int i = 0; i < trainParameters.generations; i++)
			{
				//train
				stockNNES.Environment = new StockEnvironment(trainParameters.stockName, trainParameters.inputSize, trainParameters.ticksTrain, trainParameters.ticksTest + trainParameters.inputSize - 1);

				for (int j = 0; j < trainParameters.ticksTrain; j++)
				{
					stockNNES.RunTick();
				}
				KeyValuePair<EvolutionalEntity<Action>, float> bestPerformerTrain = stockNNES.Performances.OrderBy(p => p.Value).Last();
				trainStatistics.Add(new NNStockStatistics { EvolutionalEntity = bestPerformerTrain.Key, Performance = bestPerformerTrain.Value, Generation = i + 1 });

				trainAverage.Add(new NNStockStatistics { EvolutionalEntity = null, Performance = GetAverage(stockNNES), Generation = i + 1 });

				Dictionary<EvolutionalEntity<Action>, float> performance = new Dictionary<EvolutionalEntity<Action>, float>(stockNNES.Performances);

				//test
				stockNNES.Environment = new StockEnvironment(trainParameters.stockName, trainParameters.inputSize, trainParameters.ticksTest);
				for (int j = 0; j < trainParameters.ticksTest; j++)
				{
					stockNNES.RunTick();
				}

				KeyValuePair<EvolutionalEntity<Action>, float> bestPerformerTest = stockNNES.Performances.OrderBy(p => p.Value).Last();
				testStatistics.Add(new NNStockStatistics { EvolutionalEntity = bestPerformerTest.Key, Performance = bestPerformerTest.Value, Generation = i + 1 });

				testAverage.Add(new NNStockStatistics { EvolutionalEntity = null, Performance = GetAverage(stockNNES), Generation = i + 1 });

				GetAverage(stockNNES);

				//reset performance
				stockNNES.Performances = performance;

				if (i != trainParameters.generations - 1)
				{
					stockNNES.CreateNextGeneration();
				}
			}

			trainReturn.trainStatistics = trainStatistics;
			trainReturn.testStatistics = testStatistics;

			trainReturn.trainAverage = trainAverage;
			trainReturn.testAverage = testAverage;

			return trainReturn;
		}
	}
}