using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TransitionSystemChecker.StateFormulas;

namespace TransitionSystemChecker.PathFormulas
{
    class PUntil : PathFormula
    {
        public StateFormula left;
        public StateFormula right;

        public PUntil(StateFormula left, StateFormula right)
        {
            this.left = left;
            this.right = right;
        }

        public override PathFormula existentialNormalForm()
        {
            StateFormula e_left = left.existentialNormalForm();
            StateFormula e_right = right.existentialNormalForm();

            return new PUntil(e_left, e_right);
        }


    }
}
