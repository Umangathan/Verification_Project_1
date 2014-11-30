using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.StateFormulas
{
    class SNot : StateFormula
    {
        public StateFormula state;

        public SNot(StateFormula state)
        {
            this.state = state;
        }

        public override StateFormula existentialNormalForm()
        {
            StateFormula e_state = state.existentialNormalForm();
            return new SNot(e_state);
        }

    }
}
