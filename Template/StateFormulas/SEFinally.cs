using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modest.Teaching;

namespace TransitionSystemChecker.StateFormulas
{
    class SEFinally : StateFormula
    {
        public StateFormula operand;

        public SEFinally(StateFormula operand)
        {
            this.operand = operand;
        }

        public override string ToString()
        {
            String op = operand.ToString();

            return "E <> ( " + op + " )";
        }

        public override StateFormula existentialNormalForm()
        {
            //EF phi = E (true U phi)
            StateFormula true_state = new SBoolean(true);
            StateFormula phi = operand.existentialNormalForm();

            return new SEUntil(true_state, phi);
        }

        public override void isSatiesfied<T>(TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat, ref Pre_Compute_Factory<T> factory)
        {
            sat = new HashSet<T>();
        }
    }
}
