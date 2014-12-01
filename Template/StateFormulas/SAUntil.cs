using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.StateFormulas
{
    class SAUntil : StateFormula
    {
        public StateFormula left;
        public StateFormula right;

        public SAUntil(StateFormula left, StateFormula right)
        {
            this.left = left;
            this.right = right;
        }

        public override StateFormula existentialNormalForm()
        {
            // A(p U w) = NOT E(NOT w U (NOT p AND NOT w)) AND NOT EG NOT w
            StateFormula not_right1 = new SNot(right);
            StateFormula not_right2 = new SNot(right);
            StateFormula not_right3 = new SNot(right);

            StateFormula not_left = new SNot(left);

            StateFormula inner_and = new SAnd(not_left, not_right2);
            StateFormula exists_until = new SEUntil(not_right1, inner_and);
            StateFormula not_exists_left = new SNot(exists_until);

            StateFormula exists_always = new SEAlways(not_right3);
            StateFormula not_exists_right = new SNot(exists_always);

            StateFormula outer_and = new SAnd(not_exists_left, not_exists_right);

            return outer_and.existentialNormalForm();

        }


    }
}
