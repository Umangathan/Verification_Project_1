using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modest.Teaching;

namespace TransitionSystemChecker.StateFormulas
{
    class SANext : StateFormula
    {
        public StateFormula operand;

        public SANext(StateFormula operand)
        {
            this.operand = operand;
        }

        public override string ToString()
        {
            String op = operand.ToString();

            return "A O( " + op + " )" ;
        }

        public override StateFormula existentialNormalForm()
        {
            // AX phi = NOT E (X NOT phi)
            StateFormula e_operand = operand.existentialNormalForm();
            StateFormula not_phi = new SNot(e_operand);
            StateFormula exists_next = new SENext(not_phi);

            return new SNot(exists_next);
        }

        public override void isSatiesfied<T>(TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat)
        {
            sat = new HashSet<T>();
        }

    }
}
