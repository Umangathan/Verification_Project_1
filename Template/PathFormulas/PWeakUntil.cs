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


            StateFormula s_left = new SNot(e_left);
            StateFormula s_right = new SAnd(new SNot(e_left.existentialNormalForm()), new SNot(e_right.existentialNormalForm()));

            return new PError();
        }

    }
}

