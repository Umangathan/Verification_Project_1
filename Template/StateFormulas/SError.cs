﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.StateFormulas
{
    class SError : StateFormula
    {
        public SError()
        {

        }

        public override StateFormula existentialNormalForm()
        {
            return new SError();
        }

    }
}