using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modest.Teaching;

namespace TransitionSystemChecker
{
    class Pre_Compute_Factory<T> where T : struct, Modest.Exploration.IState<T>
    {

        public int number_states;
        public T[] states;

        public Dictionary<T, int> state_index_lookup;

        public HashSet<T> [] pre_list;
        public HashSet<T> [] post_list;

        public TransitionSystem<T> transition_system;

        public Pre_Compute_Factory(int number_states)
        {
            this.number_states = number_states;
        }

        public Pre_Compute_Factory(int number_states, HashSet<T> states, TransitionSystem<T> transition_system)
        {
            this.number_states = number_states;
            
            int i = 0;
            this.states = new T[number_states];
            state_index_lookup = new Dictionary<T, int>();

            foreach (var state in states)
            {
                this.states[i] = state;
                state_index_lookup.Add(state, i);
                i++;
            }

            this.transition_system = transition_system;

            

            

            pre_list = new HashSet<T>[number_states];
            post_list = new HashSet<T>[number_states];

            for (int j = 0; j < number_states; j++)
            {
                pre_list[j] = new HashSet<T>();
                post_list[j] = new HashSet<T>();
            }
            //Console.WriteLine("go");
            computePreList();
            computePostList();
            //Console.WriteLine("finish");
        }

        private void computePreList()
        {
            for (int i = 0; i < number_states; i++)
            {
                T state = states[i];
                
               //HashSet<T> res = new HashSet<T>();
                T successor;



                foreach (var entry in states)
                {
                    var temp = entry;
                    foreach (var transition in transition_system.GetTransitions(ref temp))
                    {
                        transition_system.GetTargetState(ref temp, transition, out successor);

                        if (successor.Equals(state))
                            pre_list[i].Add(temp);

                    }
                }

                //pre_list[i] = res;

            }

        }

        private void computePostList()
        {
            for (int i = 0; i < number_states; i++)
            {
                T state = states[i];

                //HashSet<T> res = new HashSet<T>();
                T successor;

                foreach (var transition in transition_system.GetTransitions(ref state))
                {
                    transition_system.GetTargetState(ref state, transition, out successor);

                    post_list[i].Add(successor);
                }

                //post_list[i] = res;
            }
        }

        public HashSet<T> getPreSet(ref T state)
        {
            int index;
            state_index_lookup.TryGetValue(state, out index);

            return pre_list[index];
        }

        public HashSet<T> getPostSet(ref T state)
        {
            int index;
            state_index_lookup.TryGetValue(state, out index);

            return post_list[index];
        }

    }
}
