using NN2.Evolution;
using NN2.NN;
using NN2.NNEvolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NN2.StockTrainer
{
	public enum Action
	{
		BUY,
		SELL,
		DO_NOTHING
	}

	public struct State
	{
		public float Cash { get; set; }
		public float Stocks { get; set; }
	}

	public class StockNNES : EvolutionSimulator<Action, State>
	{
		public CryptoHistory CryptoHistory { get; set; }

		public StockNNES(SequentialNN model, int populationSize)
		{
			State defaultState = new State();
			defaultState.Cash = 1000;
			defaultState.Stocks = 0;
			DefaultState = defaultState;

			CreateInitialPopulation(model, populationSize);
		}

		private void CreateInitialPopulation(SequentialNN model, int populationSize)
		{
			//create initial population
			Population = new List<EvolutionalEntity<Action>>();
			for (int i = 0; i < populationSize; i++)
			{
				//copy population based on model
				SequentialNN newNN = new SequentialNN();
				foreach (Layer layer in model.Layers)
				{
					Layer newLayer = new Layer(layer.Perceptrons.Count);
					newNN.AddLayer(newLayer);
				}
				newNN.AddLayer(new Layer(typeof(Action).GetEnumNames().Count()));

				NNEvolutionalEntity<Action> entity = new NNEvolutionalEntity<Action>(newNN);

				Population.Add(entity);
				States.Add(entity, DefaultState);
			}
		}

		protected override State UpdateState(State state, HashSet<Action> actions, Dictionary<string, float> inputs)
		{
			State newState = new State();
			Action action = actions.First();
			//TODO: Wrong
			float priceOfStock = inputs["0:Close"];

			switch (action)
			{
				case Action.BUY:
					newState.Stocks = state.Stocks + state.Cash / priceOfStock;
					newState.Cash = 0;
					break;
				case Action.SELL:
					newState.Stocks = 0;
					newState.Cash = state.Cash + state.Stocks * priceOfStock;
					break;
				default:
					newState.Stocks = state.Stocks;
					newState.Cash = state.Cash;
					break;
			}

			return newState;
		}

		protected override float CalculatePerformance(State state, Dictionary<string, float> inputs)
		{
			float priceOfStock = inputs["0:Close"];
			double worth = state.Cash + state.Stocks * priceOfStock;
			double timeCost = worth * (0.000114155f * .02);
			return (float)(worth - timeCost);
		}

		public override void ResetState()
		{
			States = new Dictionary<EvolutionalEntity<Action>, State>();
			foreach(EvolutionalEntity<Action> entity in Population)
			{
				States.Add(entity, DefaultState);
			}
			Performances = new Dictionary<EvolutionalEntity<Action>, float>();
		}
	}
}
