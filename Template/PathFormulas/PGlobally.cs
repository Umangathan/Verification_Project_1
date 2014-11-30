using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.PathFormulas
{
    class PGlobally : PathFormula
    {
        public StateFormula globally;

        public PGlobally(StateFormula globally)
        {
            this.globally = globally;
        }



    }
}
