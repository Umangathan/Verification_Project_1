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

        public override string ToString()
        {
            return atomic.ToString();
        }

        public override StateFormula existentialNormalForm()
        {
            return new SAtomic(atomic);
        }


        public override void isSatiesfied<T>(TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat, ref Pre_Compute_Factory<T> factory)
        {
            if (factory.prop_sats.ContainsKey(this))
            {
                factory.prop_sats.TryGetValue(this, out sat);
                return;
            }


            HashSet<T> res = new HashSet<T>();

            foreach (var entry in states)
            {
                T state = entry;
                bool satisfied = transition_system.HasAtomicProposition(ref state, atomic.PropositionIndex);

                if(satisfied)
                    res.Add(state);

            }

            //Console.WriteLine("atomic");

            factory.prop_sats.Add(this, res);

            
            sat = res;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(SAtomic))
            {
                return ((SAtomic)obj).atomic.PropositionIndex == atomic.PropositionIndex;
            }

            return false;
        }
    }
}
