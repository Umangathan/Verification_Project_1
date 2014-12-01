using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.StateFormulas.ForAll
{
    class SANext : StateFormula
    {
        public StateFormula operand;

        public SANext(StateFormula operand)
        {
            this.operand = operand;
        }

        public override StateFormula existentialNormalForm()
        {
            // AX phi = NOT E (X NOT phi)
            StateFormula e_operand = operand.existentialNormalForm();
            StateFormula not_phi = new SNot(e_operand);
            StateFormula exists_next = new SENext(not_phi);

            return new SNot(exists_next);
        }
    }
}
