using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TransitionSystemChecker.PathFormulas;

namespace TransitionSystemChecker.StateFormulas
{
    class SForAll : StateFormula
    {
        public PathFormula path;

        public SForAll(PathFormula path)
        {
            this.path = path;
        }

        public override StateFormula existentialNormalForm()
        {
            if (path.GetType() == typeof(PWeakUntil))
            {

            }

            PathFormula e_path = path.existentialNormalForm();
            StateFormula exists = new SExists(e_path);

            return new SNot(exists);
        }

    }
}
