using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NN2.Evolution
{
	abstract public class EvolutionSimulator<A, S> where A : Enum where S : struct
	{
		public EvolutionEnvironment Environment { get; set; }
		public List<EvolutionalEntity<A>> Population { get; protected set; }
		public int Generation { get; protected set; }
		public Dictionary<EvolutionalEntity<A>, float> Performances { get; set; }
		public Dictionary<EvolutionalEntity<A>, S> States { get; set; }
		public S DefaultState { get; set; }
		public int Ticks { get; set; }

		public EvolutionSimulator()
		{
			Generation = 1;
			Ticks = 0;
			DefaultState = new S();

			Performances = new Dictionary<EvolutionalEntity<A>, float>();
			States = new Dictionary<EvolutionalEntity<A>, S>();
		}

		abstract protected S UpdateState(S state, HashSet<A> actions, Dictionary<string, float> inputs);
		abstract protected float CalculatePerformance(S state, Dictionary<string, float> inputs);
		abstract public void ResetState();

		public virtual void RunTick()
		{
			Dictionary<string, float> inputs = Environment.GetNextInputs();

			foreach (EvolutionalEntity<A> entity in Population)
			{
				HashSet<A> actions = entity.PerformActions(inputs);
				S newState = UpdateState(States[entity], actions, inputs);
				States[entity] = newState;
				float performance = CalculatePerformance(newState, inputs);
				Performances[entity] = performance;
			}
			Ticks++;
		}

		public virtual void CreateNextGeneration()
		{
			//rank population
			List<KeyValuePair<EvolutionalEntity<A>, float>> orderedPerformances = Performances.OrderBy(p => p.Value).Reverse().ToList();

			List<EvolutionalEntity<A>> populationAfterDeath = new List<EvolutionalEntity<A>>();
			//kill the weakest links and repopulate
			for (int i = 0; i < orderedPerformances.Count / 2; i++)
			{
				EvolutionalEntity<A> survivor = orderedPerformances[i].Key;

				for(int j=0; j < 2; j++)
				{
					EvolutionalEntity<A> child = survivor.CreateOffspring();
					populationAfterDeath.Add(child);
				}
			}

			Population = populationAfterDeath;

			Generation++;
			Performances = new Dictionary<EvolutionalEntity<A>, float>();
			States = new Dictionary<EvolutionalEntity<A>, S>();

			foreach (EvolutionalEntity<A> entity in Population)
			{
				States.Add(entity, DefaultState);
			}

			Ticks = 0;
		}
	}

}
