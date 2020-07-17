using NN2.Evolution;
using NN2.NN;
using NN2.NNEvolution;
using NN2.StockTrainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Action = NN2.StockTrainer.Action;

namespace NN2
{
	class Program
	{
		static void PrintPerformance(StockNNES stockNNES, EvolutionalEntity<Action> bestTrainingPerformer = null)
		{
			float sum = 0;
			foreach (KeyValuePair<EvolutionalEntity<Action>, float> performance in stockNNES.Performances.OrderBy(p => p.Value).Reverse())
			{
				float percentage = 100 * (performance.Value - stockNNES.DefaultState.Cash) / stockNNES.DefaultState.Cash;
				if(bestTrainingPerformer != null && performance.Key == bestTrainingPerformer)
				{
					Console.WriteLine("{0}->{1}\t{2}% <---- Best performer from training", stockNNES.DefaultState.Cash, performance.Value, percentage);
				}
				Console.WriteLine("{0}->{1}\t{2}%", stockNNES.DefaultState.Cash, performance.Value, percentage);
				sum += percentage;
			}
			Console.WriteLine("Average: {0}%", sum / stockNNES.Performances.Count);
		}

		static void Main(string[] args)
		{
			const int TICKS_TRAIN = 120;
			const int TICKS_TEST = 120;
			const int INPUT_HOURS = 24;

			const int GENERATION_TO_STOP_TRAINING = 50;
			const int POPULATION_SIZE = 100;

			StockEnvironment stockEnvironment;

			//build model
			SequentialNN model = new SequentialNN();
			model.AddLayer(new Layer(5));
			model.AddLayer(new Layer(5));

			StockNNES stockNNES = new StockNNES(model, POPULATION_SIZE);
			StockNNES stockNNESRandom = new StockNNES(model, POPULATION_SIZE);

			//train
			for (int i=0; i<GENERATION_TO_STOP_TRAINING; i++)
			{
				Console.Clear();
				Console.WriteLine("Training Generation: {0}", stockNNES.Generation);

				//create new environment
				stockEnvironment = new StockEnvironment("XVG", INPUT_HOURS, TICKS_TRAIN, TICKS_TEST + INPUT_HOURS - 1);
				stockNNES.Environment = stockEnvironment;

				for(int j=0; j<TICKS_TRAIN; j++)
				{
					stockNNES.RunTick();
				}
			
				if(i != GENERATION_TO_STOP_TRAINING - 1)
				{
					stockNNES.CreateNextGeneration();
				}
			}

			//print performances
			Console.WriteLine();
			Console.WriteLine("Performance of trained population");
			Console.WriteLine("----------------");
			PrintPerformance(stockNNES);

			EvolutionalEntity<Action> bestTrainingPerformer = stockNNES.Performances.OrderBy(p => p.Value).Last().Key;

			//reset performance
			stockNNES.ResetState();

			//test trained
			Console.WriteLine("Testing Trained");

			stockEnvironment = new StockEnvironment("BTC", INPUT_HOURS, TICKS_TEST);
			stockNNES.Environment = stockEnvironment;
			for (int i=0; i< TICKS_TEST; i++)
			{
				stockNNES.RunTick();
			}

			//test random
			Console.WriteLine("Testing Random");

			stockEnvironment = new StockEnvironment("BTC", INPUT_HOURS, TICKS_TEST);
			stockNNESRandom.Environment = stockEnvironment;
			for (int i = 0; i < TICKS_TEST; i++)
			{
				stockNNESRandom.RunTick();
			}

			//print performances of random population
			Console.WriteLine();
			Console.WriteLine("Performance of random population");
			Console.WriteLine("----------------");
			PrintPerformance(stockNNESRandom);

			//print performances
			Console.WriteLine();
			Console.WriteLine("Performance of trained population");
			Console.WriteLine("----------------");
			PrintPerformance(stockNNES, bestTrainingPerformer);
		}
	}
};

