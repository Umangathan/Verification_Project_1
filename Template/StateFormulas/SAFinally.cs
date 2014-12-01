using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.StateFormulas
{
    class SAFinally : StateFormula
    {
        public StateFormula operand;

        public SAFinally(StateFormula operand)
        {
            this.operand = operand;
        }

        public override StateFormula existentialNormalForm()
        {
            // AG phi = NOT EG (NOT phi)
            StateFormula e_operand = operand.existentialNormalForm();
            StateFormula not_phi = new SNot(e_operand);

            StateFormula exists_always = new SEAlways(not_phi);

            return new SNot(exists_always);
        }

    }
}
