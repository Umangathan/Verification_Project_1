using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modest.Teaching;

namespace TransitionSystemChecker
{
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

                    foreach (T entry in pre_list[i])
                        new_pre_list[i].Add(entry);

                    foreach (T entry in post_list[i])
                        new_post_list[i].Add(entry);

                    //new_pre_list[i] = pre_list[i];
                    //new_post_list[i] = post_list[i]; 
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

        public void addToPre(T state, T add) {
            int index = getIndex(state);
            //index_lookup.TryGetValue(state, out index);

            pre_list[index].Add(add);
            //Console.WriteLine("state : " + index + " now has " + pre_list[index].Count + " predecessors");

        }

        public void addToPost(T state, T add) {
            int index = getIndex(state);
            //index_lookup.TryGetValue(state, out index);

            post_list[index].Add(add);
        }

        public int getIndex(T state) {
            
            

            int index;
            index_lookup.TryGetValue(state, out index);

            //Console.WriteLine("index: " + index);

            return index;

        }

    }
}
