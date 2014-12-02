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

        public override void isSatiesfied<T>(Modest.Teaching.TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat)
        {
            HashSet<T> sat_1;
            left.isSatiesfied<T>(transition_system, states, out sat_1);
            HashSet<T> sat_2; 
            right.isSatiesfied<T>(transition_system, states, out sat_2);

            sat_1.IntersectWith(sat_2);

            sat = sat_1;
        }

    }
}
