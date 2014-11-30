using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TransitionSystemChecker.StateFormulas;

namespace TransitionSystemChecker.PathFormulas
{
    class PEventually : PathFormula
    {
        public StateFormula eventually;

        public PEventually(StateFormula eventually)
        {
            this.eventually = eventually;
        }

        public override PathFormula existentialNormalForm()
        {
            // F phi = (true U phi)

            StateFormula left = new SBoolean(true);
            StateFormula right = eventually.existentialNormalForm();

            return new PUntil(left, right);
            
        }

    }
}
