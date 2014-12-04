using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modest.Teaching;

namespace TransitionSystemChecker
{
    /*
     * This class saves various things needed for the model checking
     * This class is only used in the Pre_Compute_Factory class
     * The things this class stores are:
     *      state set which contains all states
     *      predecessor_list set for each set
     *      post_list set for each set
     * Also it has a dictionary (index_lookup)
     * which maps each state to a different integer
     * so that we can actually find things in our arrays
     */


    class DynamicArray<T>
        where T : struct, Modest.Exploration.IState<T>
    {
        public int cap;
        public int next_index;
        public HashSet<T> states;
        public Dictionary<T, int> index_lookup;
        public HashSet<T>[] pre_list;
        public HashSet<T>[] post_list;
        public static int INIT = 200;

        public DynamicArray() {
            states = new HashSet<T>();
            pre_list = new HashSet<T>[INIT];
            post_list = new HashSet<T>[INIT];

            for(int i = 0; i < INIT; i++) {
                pre_list[i] = new HashSet<T>();
                post_list[i] = new HashSet<T>();
            }

            index_lookup = new Dictionary<T, int>();

            cap = INIT;
            next_index = 0;

        }

        /*
         * This method tries to add a state to our states-array
         * If there is not enough room simply make the array larger
         * Always make sure the pre/post lists have enough space for each state
         */
         
        public bool addToStates(T state) {
            if(next_index == cap) {
                
                int old_cap = cap;
                cap = 2*cap;

                
                HashSet<T>[] new_pre_list = new HashSet<T>[cap];
                HashSet<T>[] new_post_list = new HashSet<T>[cap];

                for(int i = 0; i<cap; i++) {
                    new_pre_list[i] = new HashSet<T>();
                    new_post_list[i] = new HashSet<T>();
                }

                

                for(int i = 0; i < old_cap; i++) {

                    new_pre_list[i] = pre_list[i];
                    new_post_list[i] = post_list[i]; 

                    /*
                    foreach (T entry in pre_list[i])
                        new_pre_list[i].Add(entry);

                    foreach (T entry in post_list[i])
                        new_post_list[i].Add(entry);
                    */
                    
                }
                 



                

              
                pre_list = new_pre_list;
                post_list = new_post_list;
            } 

            bool successful = states.Add(state);
            if (successful)
            {
                index_lookup.Add(state, next_index);
                next_index++;
            }
            

            return successful;
        }


        // Adds state "add" to pre_list of state "state
        public void addToPre(T state, T add) {
            int index = getIndex(state);
            
            pre_list[index].Add(add);
        }

        // Adds state "add" to post_list of state "state"
        public void addToPost(T state, T add) {
            int index = getIndex(state);
            
            post_list[index].Add(add);
        }


        // Gets the index of state "state"
        public int getIndex(T state) {
            
            

            int index;
            index_lookup.TryGetValue(state, out index);

            return index;

        }

    }
}
