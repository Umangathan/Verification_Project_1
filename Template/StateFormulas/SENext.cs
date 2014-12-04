using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.StateFormulas
{
    class SENext : StateFormula
    {
        public StateFormula operand;

        public SENext(StateFormula operand)
        {
            this.operand = operand;
        }

        public override string ToString()
        {
            String op = operand.ToString();

            return "E O( " + op + " )";
        }

        public override StateFormula existentialNormalForm()
        {
            StateFormula e_operand = operand.existentialNormalForm();

            return new SENext(e_operand);
        }

        public override void isSatiesfied<T>(Modest.Teaching.TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat, ref Pre_Compute_Factory<T> factory)
        {

            if (factory.prop_sats.ContainsKey(this))
            {
                factory.prop_sats.TryGetValue(this, out sat);
                return;
            }
            
            HashSet<T> operand_sat;
            operand.isSatiesfied<T>(transition_system, states, out operand_sat, ref factory);
            HashSet<T> res = new HashSet<T>();

            foreach (var entry in states)
            {
                HashSet<T> post = new HashSet<T>();
                var state = entry;
                T successor;
                foreach (var transition in transition_system.GetTransitions(ref state))
                {
                    transition_system.GetTargetState(ref state, transition, out successor);
                    post.Add(successor);
                }

                post.IntersectWith(operand_sat);

                if (post.Count > 0)
                    res.Add(entry);

            }

            factory.prop_sats.Add(this, res);

            sat = res;
            
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(SENext))
            {
                return ((SENext)obj).operand.Equals(operand);
            }

            return false;
        }

    }
}
