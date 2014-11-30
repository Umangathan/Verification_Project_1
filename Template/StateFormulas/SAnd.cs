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

        public override StateFormula existentialNormalForm()
        {
           StateFormula e_left = left.existentialNormalForm();
           StateFormula e_right = right.existentialNormalForm();

           return new SAnd(e_left, e_right);
        }


    }
}
