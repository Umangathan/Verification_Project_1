using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modest.Teaching;

namespace TransitionSystemChecker.StateFormulas
{
    class SAFinally : StateFormula
    {
        public StateFormula operand;

        public SAFinally(StateFormula operand)
        {
            this.operand = operand;
        }

        public override string ToString()
        {
            String op = operand.ToString();

            return "A <> ( " + op + " )" ;
        }

        public override StateFormula existentialNormalForm()
        {
            // AG phi = NOT EG (NOT phi)
            StateFormula e_operand = operand.existentialNormalForm();
            StateFormula not_phi = new SNot(e_operand);

            StateFormula exists_always = new SEAlways(not_phi);

            return new SNot(exists_always);
        }

        public override void isSatiesfied<T>(TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat, ref Pre_Compute_Factory<T> factory)
        {
            sat = new HashSet<T>();
        }

    }
}
