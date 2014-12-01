using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransitionSystemChecker.StateFormulas
{
    class SEAlways : StateFormula
    {
        public StateFormula operand;

        public SEAlways(StateFormula operand)
        {
            this.operand = operand;
        }

        public override StateFormula existentialNormalForm()
        {
            StateFormula e_operand = operand.existentialNormalForm();

            return new SEAlways(e_operand);
        }

        public override void isSatiesfied<T>(Modest.Teaching.TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat)
        {
            HashSet<T> v;                                                                        //Sat(operand)    
            operand.isSatiesfied<T>(transition_system, states, out v);                  
            HashSet<T> e = new HashSet<T>();                                                    //S\V

            Queue<T> e_queue = new Queue<T>();

            foreach (var entry in e)
                e_queue.Enqueue(entry);

            Dictionary<T, int> state_c = new Dictionary<T, int>();

            foreach (var entry in states)
                e.Add(entry);

            e.ExceptWith(v);


            // Initialize c[s] for every state in Sat(operand)
            foreach (var entry in v)
            {
                var temp = entry;
                HashSet<T> post = StateFormula.getPostSet<T>(transition_system, ref states, ref temp);

                state_c.Add(temp, post.Count);

            }

            int e_queue_size = e_queue.Count;

            while (e_queue_size > 0)
            {
                var entry = e_queue.Dequeue();
                HashSet<T> pre_entry = StateFormula.getPreSet<T>(transition_system, ref states, ref entry);

                foreach (var s in pre_entry)
                {
                    if (v.Contains(s))
                    {
                        int current_c;
                        state_c.TryGetValue(s, out current_c);
                        state_c.Remove(s);

                        current_c--;

                        state_c.Add(s, current_c);

                        if (current_c == 0)
                        {
                            v.Remove(s);
                            e_queue.Enqueue(s);
                        }
                    }
                }

                e_queue_size = e_queue.Count;

            }

            sat = v;


        }

    }
}
