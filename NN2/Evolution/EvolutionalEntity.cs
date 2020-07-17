using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NN2.Evolution
{
	abstract public class EvolutionalEntity<A> where A : Enum
	{
		abstract public HashSet<A> PerformActions(Dictionary<string, float> inputs);
		abstract public EvolutionalEntity<A> CreateOffspring();
	}
}
