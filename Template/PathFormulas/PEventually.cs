using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.PathFormulas
{
    class PEventually : PathFormula
    {
        public StateFormula eventually;

        public PEventually(StateFormula eventually)
        {
            this.eventually = eventually;
        }

    }
}
