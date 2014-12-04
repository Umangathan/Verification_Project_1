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

        public override string ToString()
        {
            String op = operand.ToString();

            return "E[] ( " + op + " )";
        }

        public override StateFormula existentialNormalForm()
        {
            StateFormula e_operand = operand.existentialNormalForm();

            return new SEAlways(e_operand);
        }

        public override void isSatiesfied<T>(Modest.Teaching.TransitionSystem<T> transition_system, LinkedList<T> states, out HashSet<T> sat, ref Pre_Compute_Factory<T> factory)
        {
            if (factory.prop_sats.ContainsKey(this))
            {
                factory.prop_sats.TryGetValue(this, out sat);
                return;
            }


            //Console.WriteLine("Always start");
            HashSet<T> v;                                                                        //Sat(operand)    
            operand.isSatiesfied<T>(transition_system, states, out v, ref factory);                  
            HashSet<T> e = new HashSet<T>();                                                    //S\V

            foreach (var entry in states)
                e.Add(entry);

            e.ExceptWith(v);

            Queue<T> e_queue = new Queue<T>();

            foreach (var entry in e)
                e_queue.Enqueue(entry);

            Dictionary<T, int> state_c = new Dictionary<T, int>();



            // Initialize c[s] for every state in Sat(operand)
            foreach (var entry in v)
            {
                var temp = entry;
                HashSet<T> post = factory.getPostSet(ref temp);
                
                state_c.Add(temp, post.Count);

            }

            int e_queue_size = e_queue.Count;

            while (e_queue_size > 0)
            {
                var s_prime = e_queue.Dequeue();
                HashSet<T> pre_s_prime = factory.getPreSet(ref s_prime);

                foreach (var s in pre_s_prime)
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

            factory.prop_sats.Add(this, v);

            sat = v;

            //Console.WriteLine("Always stop");

        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(SEAlways))
            {
                return ((SEAlways)obj).operand.Equals(operand);
            }

            return false;
        }

    }
}
