﻿using System;
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

        public override StateFormula existentialNormalForm()
        {
            // AG phi = NOT EF (NOT phi)
            StateFormula e_operand = operand.existentialNormalForm();

            StateFormula not_phi = new SNot(e_operand);
            StateFormula exists_finally = new SEFinally(not_phi);

            return new SNot(exists_finally);

            
        }

        public override void isSatiesfied<T>(TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat)
        {
            sat = new HashSet<T>();
        }

    }
}
