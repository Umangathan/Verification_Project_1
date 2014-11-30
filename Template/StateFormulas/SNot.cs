using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.StateFormulas
{
    class SNot : StateFormula
    {
        public StateFormula path;

        public SNot(StateFormula path)
        {
            this.path = path;
        }

    }
}
