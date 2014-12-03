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
           

            compute_pre_post_list();
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



        private void compute_pre_post_list() {
            Queue<T> to_be_expanded = new Queue<T>();
            HashSet<T> already_expanded = new HashSet<T>();

            T initialState;
            transition_system.GetInitialState(out initialState);

            to_be_expanded.Enqueue(initialState);
            already_expanded.Add(initialState);

            int size = to_be_expanded.Count;

            //var newestStates = new List<T>();

            int count = 0;

            while (size > 0)
            {
                
                
                var state = to_be_expanded.Dequeue();

                int state_index;
                state_index_lookup.TryGetValue(state, out state_index);

                T successor;

                foreach (var transition in transition_system.GetTransitions(ref state))
                {
                    transition_system.GetTargetState(ref state, transition, out successor);

                    // Add successor to post_list of state
                    post_list[state_index].Add(successor);

                    // Add state to pre_list of successor
                    int successor_index;
                    state_index_lookup.TryGetValue(successor, out successor_index);
                    pre_list[successor_index].Add(state);

                    
                    if (already_expanded.Add(successor))
                    {
                        to_be_expanded.Enqueue(successor);
                        //newestStates.Add(successor);
                    }

                    
                }

                count++;

                size = to_be_expanded.Count;

                // If no transition has been encountered for this state it was terminal

            }
        }

    }
}
