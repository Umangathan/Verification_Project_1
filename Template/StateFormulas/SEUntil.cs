using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.StateFormulas
{
    class SEUntil : StateFormula
    {
        public StateFormula left;
        public StateFormula right;

        public SEUntil(StateFormula left, StateFormula right)
        {
            this.left = left;
            this.right = right;
        }

        public override string ToString()
        {
            String l = left.ToString();
            String r = right.ToString();

            return "E ( " + "( " + l + " )" + " U " + "( " + r + " )" + " )";
        }

        public override StateFormula existentialNormalForm()
        {
            StateFormula e_left = left.existentialNormalForm();
            StateFormula e_right = right.existentialNormalForm();

            return new SEUntil(e_left, e_right);
        }

        public override void isSatiesfied<T>(Modest.Teaching.TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat)
        {
            HashSet<T> t;                                                                    //Sat(right)
            right.isSatiesfied<T>(transition_system, states, out t);
            List<T> e;                                                                       //Sat(right) <-- still to be expanded           
            HashSet<T> temp;
            right.isSatiesfied<T>(transition_system, states, out temp);
            e = temp.ToList<T>();
            Queue<T> e_queue = new Queue<T>();

            HashSet<T> left_sat;                                                              //Sat(left)
            left.isSatiesfied<T>(transition_system, states, out left_sat);


            foreach(var entry in e)
                e_queue.Enqueue(entry);

            HashSet<T> x;
            


            int e_queue_size = e_queue.Count;

            while (e_queue_size > 0)
            {
                var s_prime = e_queue.Dequeue();
                HashSet<T> pre = StateFormula.getPreSet<T>(transition_system, ref states, ref s_prime);
                left_sat.ExceptWith(t);

                foreach (var s in pre)
                {
                    if (left_sat.Contains(s))
                    {
                        t.Add(s);
                        e_queue.Enqueue(s);
                    }
                }
                e_queue_size = e_queue.Count;
                
            }

            sat = t;
        }

    }
}
