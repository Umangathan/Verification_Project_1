using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modest.Teaching;
using CI = System.Globalization.CultureInfo;

namespace TransitionSystemChecker
{
	class Checker: ITransitionSystemAnalyzer
	{
		public Checker() // called from Program.cs
		{
			// Perform initialisation work that does not depend on command-line arguments and that cannot fail here if necessary.
		}

		public bool ParseCommandLine(string[] args) // called from Program.cs
		{
			// Parse additional command-line parameters here if necessary.
			// args[0] is used for the model input file name in Program.cs and should be ignored.
			// If the given parameters are not valid, report an error on Console.Error and return false; otherwise, return true.
			return true;
		}

		public void AnalyzeTransitionSystem<T>(TransitionSystem<T> transitionSystem, ModelProperty[] properties) // called from Program.cs
			where T: struct, Modest.Exploration.IState<T>
		{   /*
			// Implement your transition system analysis procedures here
			// For illustration, let's count the immediate successors of the initial state:
			var successors = new HashSet<T>(); // the state types implement proper .GetHashCode and .Equals methods based on structural equality
			T initialState, successorState;
			transitionSystem.GetInitialState(out initialState);
			foreach(var transition in transitionSystem.GetTransitions(ref initialState))
			{
				transitionSystem.GetTargetState(ref initialState, transition, out successorState);
				successors.Add(successorState);
				// We could evaluate properties using transitionSystem.HasAtomicProposition(ref successorState, ...) here;
				// also see the class diagram for properties (file Properties-Classes.png).
			}
			Console.WriteLine("The initial state has " + successors.Count.ToString(CI.InvariantCulture) + " distinct immediate successor state" + (successors.Count == 1 ? string.Empty : "s") + ".");
            */

            // Write Tests into Files
            //var testAnalyzer = new TestAnalyzer();
            //testAnalyzer.writeTestFiles(transitionSystem, properties);

            T initialState;
            transitionSystem.GetInitialState(out initialState);
            bool terminalStatesEncountered = false;

           

            //Console.WriteLine("States: " + 4000001 + "\n");
            //Console.WriteLine("Error: deadlocks detected\n");
            //Environment.Exit(0);
            

            //Console.WriteLine("State: " + initialState.ToString() + "Transition Count: " + transitionSystem.GetTransitions(ref initialState).Count().ToString());
            // Count states and find terminal states
            var states = new List<T>();
            var newStates = new Queue<T>();
            newStates.Enqueue(initialState);
            
            //while(newStates.Count > 0)
                traverseTree<T>(transitionSystem, ref states, ref newStates, ref initialState, ref terminalStatesEncountered);

            
            Console.WriteLine("States: " + states.Count.ToString(CI.InvariantCulture) + "\n");



            if (terminalStatesEncountered)
            {
                Console.WriteLine("Error: deadlocks detected\n");
            }

            Environment.Exit(0);

		}

        public void traverseTree<T>(TransitionSystem<T> transitionSystem, ref List<T> oldStates, ref Queue<T> newStates, ref T state_a, ref bool terminalStatesEncountered)
            where T : struct, Modest.Exploration.IState<T>
        {

            //HashSet<T> successors = new HashSet<T>();

            int newSize = newStates.Count;

            //var newestStates = new List<T>();

            while(newSize > 0)
            {
                int count = 0;
                var state = newStates.Dequeue();

                //if (!(oldStates.Contains(state)))
                oldStates.Add(state);
                T successor;

                foreach (var transition in transitionSystem.GetTransitions(ref state))
                {   
                    transitionSystem.GetTargetState(ref state, transition, out successor);
                    if (!(oldStates.Contains(successor)))
                    {
                        if(!(oldStates.Contains(successor)) && !(newStates.Contains(successor)))
                            newStates.Enqueue(successor);
                        //newestStates.Add(successor);
                    }

                    count++;
                }

                newSize = newStates.Count;

                // If no transition has been encountered for this state it was terminal
                if (count == 0)
                    terminalStatesEncountered = true;
            }
            /*
            newStates.Clear();
            foreach (var state in newestStates)
            {
                if(newStates.Contains(state))
                    continue;
                newStates.Add(state);
            }
            */
            
        }

        public void checkIfCTL(ModelProperty modelProperty)
        {
            String name = modelProperty.Name;
            Property property = modelProperty.Property;

            bool supported = parseProperty(property);
            
        }

        public bool parseProperty(Property property)
        {
            String ps = property.ToString();
            if (ps.Equals("true") || ps.Equals("false") || ps.Equals("deadlock"))
                return true;

            return true;
        }
	}
}
