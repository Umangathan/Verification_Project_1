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
        //public T[] states;
        //public HashSet<T> states;

        //public Dictionary<T, int> state_index_lookup;

        //public HashSet<T> [] pre_list;
        //public HashSet<T> [] post_list;

        //public List<HashSet<T>> pre_list;
        //public List<HashSet<T>> post_list;

        public DynamicArray<T> array;

        public TransitionSystem<T> transition_system;

        public Pre_Compute_Factory(int number_states)
        {
            this.number_states = number_states;
        }

        public Pre_Compute_Factory(TransitionSystem<T> transition_system)
        {
            //this.number_states = number_states;
            //this.states = new HashSet<T>();
            this.terminal_encountered = false;
            this.array = new DynamicArray<T>();

            //int i = 0;
            //this.states = new T[number_states];
            //state_index_lookup = new Dictionary<T, int>();

            /*
            foreach (var state in states)
            {
                //this.states[i] = state;
                state_index_lookup.Add(state, i);
                i++;
            }
            */
             
            this.transition_system = transition_system;



            //pre_list = new List<HashSet<T>>();
            //post_list = new List<HashSet<T>>();

            /*
            for (int j = 0; j < number_states; j++)
            {
                pre_list[j] = new HashSet<T>();
                post_list[j] = new HashSet<T>();
            }
             */
           

            compute_pre_post_list();

            this.number_states = array.next_index - 1;
            
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

        private void compute_pre_post_list() {
            Queue<T> to_be_expanded = new Queue<T>();
            //HashSet<T> already_expanded = new HashSet<T>();

            T initialState;
            transition_system.GetInitialState(out initialState);

            to_be_expanded.Enqueue(initialState);
            //this.states.Add(initialState);
            array.addToStates(initialState);

            //Console.WriteLine("initialstate index: " + array.getIndex(initialState));

            int size = to_be_expanded.Count;

            //var newestStates = new List<T>();

            //int state_index = 0;

            while (size > 0)
            {
                int count = 0;    
                
                var state = to_be_expanded.Dequeue();

                //int state_index;
                //state_index_lookup.Add(state, state_index);

                T successor;

                foreach (var transition in transition_system.GetTransitions(ref state))
                {
                   
                    

                    count++;
                    transition_system.GetTargetState(ref state, transition, out successor);

                    if(array.addToStates(successor))
                    {
                        //Console.WriteLine("State: " + array.getIndex(successor) + " is a new state");
                        to_be_expanded.Enqueue(successor);
                        //newestStates.Add(successor);
                    }

                    // Add successor to post_list of state
                    //post_list.Add(new HashSet<T>());
                    //post_list[state_index].Add(successor);

                    int suc_ind = array.getIndex(successor);
                    int state_ind = array.getIndex(state);

                    //Console.WriteLine("State : " + suc_ind + " has been added to state " + state_ind + "'s post_list");

                    array.addToPost(state, successor);



                    // Add state to pre_list of successor
                    //int successor_index;
                    //state_index_lookup.TryGetValue(successor, out successor_index);

                    //pre_list.Add(new HashSet<T>());
                    //pre_list[successor_index].Add(state);

                    //Console.WriteLine("State : " + state_ind + " has been added to state " + suc_ind + "'s pre_list");

                    array.addToPre(successor, state);


                    
                    //if (states.Add(successor))
                    /*
                    if(array.addToStates(successor))
                    {
                        Console.WriteLine("State: " + array.getIndex(successor) + " is a new state");
                        to_be_expanded.Enqueue(successor);
                        //newestStates.Add(successor);
                    }
                    */
                    //state_index++;
                    
                }

                if (count == 0)
                    terminal_encountered = true;

                int ind = array.getIndex(state);
                //Console.WriteLine("state: " + ind + " has " + count + " transitions" );

                size = to_be_expanded.Count;

                /*
                for (int i = 0; i < states.Count; i++)
                    states[i] = already_expanded[i];
                */
                // If no transition has been encountered for this state it was terminal

            }
        }

    }
}
