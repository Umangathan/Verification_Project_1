using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.StateFormulas
{
    class SAnd : StateFormula
    {

        public StateFormula left;
        public StateFormula right;

        public SAnd(StateFormula left, StateFormula right)
        {
            this.left = left;
            this.right = right;
        }

        public override string ToString()
        {
            String l = left.ToString();
            String r = right.ToString();

            return "( " + l + " )" + " && " + "( " + r + " )"; 
        }

        public override StateFormula existentialNormalForm()
        {
           StateFormula e_left = left.existentialNormalForm();
           StateFormula e_right = right.existentialNormalForm();

           return new SAnd(e_left, e_right);
        }

        public override void isSatiesfied<T>(Modest.Teaching.TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat, ref Pre_Compute_Factory<T> factory)
        {

            if (factory.prop_sats.ContainsKey(this))
            {
                factory.prop_sats.TryGetValue(this, out sat);
                return;
            }

            HashSet<T> sat_1;
            left.isSatiesfied<T>(transition_system, states, out sat_1, ref factory);
            HashSet<T> sat_2; 
            right.isSatiesfied<T>(transition_system, states, out sat_2, ref factory);

            sat_1.IntersectWith(sat_2);

            factory.prop_sats.Add(this, sat_1);

            sat = sat_1;
                
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(SAnd))
            {
                SAnd and = (SAnd) obj;
                if (and.left.Equals(left) && and.right.Equals(right))
                    return true;
            }

            return false;

        }

    }
}
