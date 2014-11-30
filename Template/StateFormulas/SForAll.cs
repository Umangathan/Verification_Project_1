using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.StateFormulas
{
    class SForAll : StateFormula
    {
        public PathFormula path;

        public SForAll(PathFormula path)
        {
            this.path = path;
        }

    }
}
