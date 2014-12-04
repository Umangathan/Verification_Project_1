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

        public override string ToString()
        {
            String s = state.ToString();

            return "!" + s ; 
        }

        public override StateFormula existentialNormalForm()
        {
            StateFormula e_state = state.existentialNormalForm();
            return new SNot(e_state);
        }

        public override void isSatiesfied<T>(Modest.Teaching.TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat, ref Pre_Compute_Factory<T> factory)
        {
            if (factory.prop_sats.ContainsKey(this))
            {
                factory.prop_sats.TryGetValue(this, out sat);
                return;
            }

            HashSet<T> positive_sat;
            state.isSatiesfied<T>(transition_system, states, out positive_sat, ref factory);

            HashSet<T> res = new HashSet<T>();

            /*
            foreach (var entry in states)
            {
                if (positive_sat.Contains(entry))
                    continue;

                res.Add(entry);
            }
            */

            foreach (var entry in states)
                res.Add(entry);

            res.ExceptWith(positive_sat);

            factory.prop_sats.Add(this, res);

            sat = res;


        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(SNot))
            {
                return ((SNot)obj).state.Equals(state);
            }

            return false;
        }

    }
}
