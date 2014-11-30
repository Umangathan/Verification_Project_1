using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.StateFormulas
{
    class SExists : StateFormula
    {
        public PathFormula path;

        public SExists(PathFormula path)
        {
            this.path = path;
        }

    }
}
