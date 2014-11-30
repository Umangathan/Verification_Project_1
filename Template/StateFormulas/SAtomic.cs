using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modest.Teaching;
using CI = System.Globalization.CultureInfo;

namespace TransitionSystemChecker.StateFormulas
{
    class SAtomic : StateFormula
    {
        public AtomicProposition atomic;

        public SAtomic(AtomicProposition atomic)
        {
            this.atomic = atomic;
        }
    }
}
