using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TransitionSystemChecker.StateFormulas;

namespace TransitionSystemChecker.PathFormulas
{
    class PWeakUntil : PathFormula
    {
        public StateFormula left;
        public StateFormula right;

        public PWeakUntil(StateFormula left, StateFormula right)
        {
            this.left = left;
            this.right = right;
        }


        public override PathFormula existentialNormalForm()
        {
            StateFormula e_left = left.existentialNormalForm();
            StateFormula e_right = right.existentialNormalForm();


            StateFormula left = new SNot(left);
            StateFormula right = new SAnd(new SNot(left.existentialNormalForm()), new SNot(right.existentialNormalForm()));
        }

    }
}

