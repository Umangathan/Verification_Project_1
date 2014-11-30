using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TransitionSystemChecker.StateFormulas;

using Modest.Teaching;
using CI = System.Globalization.CultureInfo;

namespace TransitionSystemChecker
{
    abstract class PathFormula
    {
        abstract public PathFormula existentialNormalForm();

    }
}
