using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modest.Teaching;

namespace TransitionSystemChecker.StateFormulas
{
    class SError : StateFormula
    {
        public SError()
        {

        }

        public override string ToString()
        {
            return "(SError)";
        }

        public override StateFormula existentialNormalForm()
        {
            return new SError();
        }

        public override void isSatiesfied<T>(TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat, ref Pre_Compute_Factory<T> factory)
        {
            sat = new HashSet<T>();
        }
    }
}
