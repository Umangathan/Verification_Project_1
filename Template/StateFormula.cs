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
        abstract public void isSatiesfied<T>(TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat)
            where T : struct, Modest.Exploration.IState<T>;
       
 
        public static HashSet<T> getPreSet<T>(TransitionSystem<T> transition_system, ref LinkedList<T> states, ref T state) 
            where T : struct, Modest.Exploration.IState<T>
        {
            HashSet<T> res = new HashSet<T>();
            T successor;

            foreach (var entry in states)
            {
                var temp = entry;
                foreach(var transition in transition_system.GetTransitions(ref temp)) {
                    transition_system.GetTargetState(ref temp, transition, out successor);

                    if (successor.Equals(state))
                        res.Add(temp);

                }
            }

            return res;
        }

        public static HashSet<T> getPostSet<T>(TransitionSystem<T> transition_system, ref LinkedList<T> states, ref T state)
            where T : struct, Modest.Exploration.IState<T>
        {
            HashSet<T> res = new HashSet<T>();
            T successor;

            foreach (var transition in transition_system.GetTransitions(ref state))
            {
                transition_system.GetTargetState(ref state, transition, out successor);

                res.Add(successor);
            }

            return res;
        }

       
    }
}
