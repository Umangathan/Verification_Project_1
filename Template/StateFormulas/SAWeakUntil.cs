using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.StateFormulas.ForAll
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

    }
}
