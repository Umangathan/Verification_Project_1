using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.PathFormulas
{
    class PNext : PathFormula
    {
        public StateFormula next;

        public PNext(StateFormula next)
        {
            this.next = next;
        }



    }
}
