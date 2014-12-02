using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modest.Teaching;

namespace TransitionSystemChecker.StateFormulas
{
    class SEWeakUntil : StateFormula
    {
        public StateFormula left;
        public StateFormula right;

        public SEWeakUntil(StateFormula left, StateFormula right)
        {
            this.left = left;
            this.right = right;
        }

        public override string ToString()
        {
            String l = left.ToString();
            String r = right.ToString();

            return "E ( " + "( " + l + " )" + " W " + "( " + r + " )" + " )";
        }

        public override StateFormula existentialNormalForm()
        {
            // E(p W w) = NA((Nw) U (Np AND Nw))

            StateFormula not_right1 = new SNot(right);
            StateFormula not_right2 = new SNot(right);

            StateFormula not_left = new SNot(left);

            StateFormula and = new SAnd(not_left, not_right2);

            StateFormula forall_until = new SAUntil(not_right1, and);

            StateFormula not_forall_until = new SNot(forall_until);

            return not_forall_until.existentialNormalForm();

        }


        public override void isSatiesfied<T>(TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat)
        {
            sat = new HashSet<T>();
        }
    }
}
