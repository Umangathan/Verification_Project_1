using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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



    }
}
