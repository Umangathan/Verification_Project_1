using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    }
}
