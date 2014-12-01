using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.StateFormulas
{
    class SNot : StateFormula
    {
        public StateFormula state;

        public SNot(StateFormula state)
        {
            this.state = state;
        }

        public override StateFormula existentialNormalForm()
        {
            StateFormula e_state = state.existentialNormalForm();
            return new SNot(e_state);
        }

        public override void isSatiesfied<T>(Modest.Teaching.TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat)
        {
            HashSet<T> positive_sat;
            state.isSatiesfied<T>(transition_system, states, out positive_sat);

            HashSet<T> res = new HashSet<T>();

            foreach (var entry in states)
            {
                if (positive_sat.Contains(entry))
                    continue;

                res.Add(entry);
            }

            sat = res;

        }

    }
}
