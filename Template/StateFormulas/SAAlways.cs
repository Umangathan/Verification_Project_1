using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modest.Teaching;

namespace TransitionSystemChecker.StateFormulas
{
    class SAAlways : StateFormula
    {
        public StateFormula operand;

        public SAAlways(StateFormula operand)
        {
            this.operand = operand;
        }

        public override string ToString()
        {
            String op = operand.ToString();

            return "A[] ( " + op + " )";
        }

        public override StateFormula existentialNormalForm()
        {
            // AG phi = NOT EF (NOT phi) = NOT E (true U (NOT phi))
            StateFormula e_operand = operand.existentialNormalForm();

            StateFormula not_phi = new SNot(e_operand);
            StateFormula true_state = new SBoolean(true);
            StateFormula exists_until = new SEUntil(true_state, not_phi);
            

            return new SNot(exists_until);

            
        }

        public override void isSatiesfied<T>(TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat, ref Pre_Compute_Factory<T> factory)
        {
            sat = new HashSet<T>();
        }

    }
}
