using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modest.Teaching;
using CI = System.Globalization.CultureInfo;

namespace TransitionSystemChecker
{
    abstract class StateFormula
    {
        

        abstract public StateFormula existentialNormalForm();
        
       
    }
}
