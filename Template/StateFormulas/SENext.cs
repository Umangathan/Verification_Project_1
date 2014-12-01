using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.StateFormulas
{
    class SENext : StateFormula
    {
        public StateFormula operand;

        public SENext(StateFormula operand)
        {
            this.operand = operand;
        }

        public override StateFormula existentialNormalForm()
        {
            StateFormula e_operand = operand.existentialNormalForm();

            return new SENext(e_operand);
        }

    }
}
