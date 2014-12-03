using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.StateFormulas
{
    class SBoolean : StateFormula
    {
        public bool b;

        public SBoolean(bool b)
        {
            this.b = b;
        }

        public override string ToString()
        {
            return b.ToString();
        }

        public override StateFormula existentialNormalForm()
        {
            return new SBoolean(b);
        }


        public override void isSatiesfied<T>(Modest.Teaching.TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat, ref Pre_Compute_Factory<T> factory)
        {
            if (b)
            {
                var res = new HashSet<T>();
                foreach (var entry in states)
                    res.Add(entry);
                sat = res;
            }
            else
            {
                sat = new HashSet<T>();
            }
        }



       
    }
}
