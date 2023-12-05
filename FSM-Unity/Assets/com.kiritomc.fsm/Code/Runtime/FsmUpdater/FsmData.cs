using System.Collections.Generic;

namespace FSM.Runtime
{
    public struct FsmData //ToDo: rename
    {
        public readonly IEnumerable<IFsmAgent> Agents;

        public FsmData(IEnumerable<IFsmAgent> agents)
        {
            Agents = agents;
        }
    }
}