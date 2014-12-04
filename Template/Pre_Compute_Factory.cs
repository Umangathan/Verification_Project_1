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

        private DynamicArray<T> array;                  // Saves alot of data; for more info go to that class

        public TransitionSystem<T> transition_system;

        
        /* When the satisfaction set of a state formula
         * is computed for the first time 
         * it saves that set in this dictionary
         * We use the StateFormula itself as a key
         * Proper Equals methods have been implented for 
         * that purpose (Only for ENF formulas though
         * since we will not try to get the sat set 
         * anywhere else
         * So the next time that set does not have to 
         * be computed again
         */
        public Dictionary<StateFormula, HashSet<T>> prop_sats;      

        // Just a debug constructor 
        public Pre_Compute_Factory(int number_states)
        {
            this.number_states = number_states;
        }

        // Returns the state set which contains all states
        public HashSet<T> getStates() {
            return array.states;
        }

        public Pre_Compute_Factory(TransitionSystem<T> transition_system)
        {
            this.terminal_encountered = false;
            this.array = new DynamicArray<T>();
            this.prop_sats = new Dictionary<StateFormula, HashSet<T>>();
             
            this.transition_system = transition_system;

            // Count states, set pre/post sets
            compute_pre_post_list();

            this.number_states = array.next_index;
            
        }

        public HashSet<T> getPreSet(ref T state)
        {
            int index = array.getIndex(state);

            return array.pre_list[index];
        }

        public HashSet<T> getPostSet(ref T state)
        {
            int index = array.getIndex(state);

            return array.post_list[index];
        }


        public bool terminal_encountered;

        /* This function counts states and
         * precomputes the post/pre sets 
         * for each state
         * Therefore it simply traverses the tree 
         * and expands the new found nodes
         * The counter "count" counts the outgoing transitions 
         * of the current state to determine
         * if it is a terminal state
         * If it was we set terminal_encountered to true
        */
        private void compute_pre_post_list() {
            Queue<T> to_be_expanded = new Queue<T>();
            
            T initialState;
            transition_system.GetInitialState(out initialState);

            to_be_expanded.Enqueue(initialState);            
            array.addToStates(initialState);

            int size = to_be_expanded.Count;

            while (size > 0)
            {
                int count = 0;    
                
                var state = to_be_expanded.Dequeue();

                T successor;

                foreach (var transition in transition_system.GetTransitions(ref state))
                {
                   
                    count++;
                    transition_system.GetTargetState(ref state, transition, out successor);

                    if(array.addToStates(successor))
                    {
                        to_be_expanded.Enqueue(successor);
                    }
                    
                    int suc_ind = array.getIndex(successor);
                    int state_ind = array.getIndex(state);

                    array.addToPost(state, successor);

                    array.addToPre(successor, state);


                }

                if (count == 0)
                    terminal_encountered = true;

                int ind = array.getIndex(state);

                size = to_be_expanded.Count;

            }
        }

    }
}
