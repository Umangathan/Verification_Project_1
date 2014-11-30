using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.StateFormulas
{
    class SBoolean : StateFormula
    {
        public bool b;

        public SBoolean(bool b)
        {
            this.b = b;
        }

        public override StateFormula existentialNormalForm()
        {
            return new SBoolean(b);
        }
    }
}
