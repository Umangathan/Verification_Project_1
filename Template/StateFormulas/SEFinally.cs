using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.StateFormulas
{
    class SEFinally : StateFormula
    {
        public StateFormula operand;

        public SEFinally(StateFormula operand)
        {
            this.operand = operand;
        }

        public override StateFormula existentialNormalForm()
        {
            //EF phi = E (true U phi)
            StateFormula true_state = new SBoolean(true);
            StateFormula phi = operand.existentialNormalForm();

            return new SEUntil(true_state, phi);
        }
    }
}
