using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modest.Teaching;

namespace TransitionSystemChecker.StateFormulas
{
    class SAWeakUntil : StateFormula
    {
        public StateFormula left;
        public StateFormula right;

        public SAWeakUntil(StateFormula left, StateFormula right)
        {
            this.left = left;
            this.right = right;
        }

        public override string ToString()
        {
            String l = left.ToString();
            String r = right.ToString();

            return "A ( " + "( " + l + " )" + " W " + "( " + r + " )" + " )";
        }

        public override StateFormula existentialNormalForm()
        {
            // E(p W w) = NE((Nw) U (Np AND Nw))

            StateFormula not_right1 = new SNot(right);
            StateFormula not_right2 = new SNot(right);

            StateFormula not_left = new SNot(left);

            StateFormula and = new SAnd(not_left, not_right2);

            StateFormula exists_until = new SEUntil(not_right1, and);

            StateFormula not_exists_until = new SNot(exists_until);

            return not_exists_until.existentialNormalForm();
        }

        public override void isSatiesfied<T>(TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat, ref Pre_Compute_Factory<T> factory)
        {
            sat = new HashSet<T>();
        }

    }
}
