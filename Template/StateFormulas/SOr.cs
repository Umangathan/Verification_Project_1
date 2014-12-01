using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modest.Teaching;

namespace TransitionSystemChecker.StateFormulas
{
    class SOr : StateFormula
    {
        public StateFormula left;
        public StateFormula right;

        public SOr(StateFormula left, StateFormula right)
        {
            this.left = left;
            this.right = right;
        }

        public override StateFormula existentialNormalForm()
        {
            StateFormula e_left = left.existentialNormalForm();
            StateFormula e_right = right.existentialNormalForm();

            // a OR b = NOT ((NOT a) AND (NOT b))
            e_left = new SNot(e_left);
            e_right = new SNot(e_right);

            StateFormula and = new SAnd(e_left, e_right);

            return new SNot(and);



        }

        public override void isSatiesfied<T>(TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat)
        {
            sat = new HashSet<T>();
        }

    }
}
