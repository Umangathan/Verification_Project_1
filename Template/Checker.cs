using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modest.Teaching;
using TransitionSystemChecker.StateFormulas;
using CI = System.Globalization.CultureInfo;

namespace TransitionSystemChecker
{
    class Checker : ITransitionSystemAnalyzer
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
            where T : struct, Modest.Exploration.IState<T>
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
            //testAnalyzer.writeTestFiles<T>(transitionSystem, properties);

            T initialState;
            transitionSystem.GetInitialState(out initialState);
            bool terminalStatesEncountered = false;



            //Console.WriteLine("States: " + 4000001 + "\n");
            //Console.WriteLine("Error: deadlocks detected\n");
            //Environment.Exit(0);


            //Console.WriteLine("State: " + initialState.ToString() + "Transition Count: " + transitionSystem.GetTransitions(ref initialState).Count().ToString());
            // Count states and find terminal states
            var states = new HashSet<T>();
            var newStates = new Queue<T>();
            newStates.Enqueue(initialState);
            states.Add(initialState);

            //while(newStates.Count > 0)
            traverseTree<T>(transitionSystem, ref states, ref newStates, ref initialState, ref terminalStatesEncountered);


            Console.WriteLine("States: " + states.Count.ToString(CI.InvariantCulture) + "\n");



            if (terminalStatesEncountered)
            {
                Console.WriteLine("Error: deadlocks detected\n");
            }

            // Transform properties in State_Formulas;
            var state_formulas = new Dictionary<String, StateFormula>();

            foreach (var model_property in properties)
            {
                Property property = model_property.Property; 
                String name = model_property.Name;
                StateFormula complete_formula = new SError();
                bool is_ctl = true;


                parseStateFormula(property, out complete_formula, ref is_ctl);


                if (!is_ctl)
                {
                    Console.WriteLine(name + ": not supported \n");
                }
                else
                {
                    state_formulas.Add(name, complete_formula);

                }
            }

            //Transform into ENF

            var state_ENF = new Dictionary<String, StateFormula>();
            foreach (var key_formula in state_formulas)
            {

                 state_ENF.Add(key_formula.Key, key_formula.Value.existentialNormalForm());
            }


            // Now do the model checking
            foreach (var entry in state_ENF)
            {
                String name = entry.Key;
                StateFormula state_formula = entry.Value;

                bool isSatisfied;
                LinkedList<T> linked_states = new LinkedList<T>();
                foreach (var entry_state in states)
                    linked_states.AddLast(entry_state);

                ModelChecker<T>(transitionSystem, linked_states, state_formula, out isSatisfied);

                if (isSatisfied)
                {
                    Console.WriteLine(name + ": true \n");
                }
                else
                {
                    Console.WriteLine(name + ": false \n");
                }

                


            }


             // Environment.Exit(0);

        }

        public void traverseTree<T>(TransitionSystem<T> transitionSystem, ref HashSet<T> already_expanded, ref Queue<T> newStates, ref T state_a, ref bool terminalStatesEncountered)
            where T : struct, Modest.Exploration.IState<T>
        {

            //HashSet<T> successors = new HashSet<T>();

            int newSize = newStates.Count;

            //var newestStates = new List<T>();

            while (newSize > 0)
            {
                //Console.WriteLine(newSize);
                //Console.WriteLine(newSize);
                int count = 0;
                var state = newStates.Dequeue();

                //if (!(oldStates.Contains(state)))
                //already_expanded.Add(state);
                T successor;

                foreach (var transition in transitionSystem.GetTransitions(ref state))
                {
                    transitionSystem.GetTargetState(ref state, transition, out successor);

                    if (already_expanded.Add(successor))
                    {
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


        }


        public void parseStateFormula(Property property, out StateFormula result, ref bool is_ctl)
        {
            StateFormula state1, state2;
            Property property1, property2;
            int temp;


            if (property.GetType() == typeof(True))
            {
                result = new SBoolean(true);
            }
            else if (property.GetType() == typeof(False))
            {
                result = new SBoolean(false);
            }
            else if (property.GetType() == typeof(AtomicProposition))
            {

                result = new SAtomic((AtomicProposition)property);
            }
            else if (property.GetType() == typeof(And))
            {
                property1 = ((And)property).LeftOperand;
                property2 = ((And)property).RightOperand;

                parseStateFormula(property1, out state1, ref is_ctl);
                parseStateFormula(property2, out state2, ref is_ctl);

                result = new SAnd(state1, state2);
            }
            else if (property.GetType() == typeof(Or))
            {
                property1 = ((Or)property).LeftOperand;
                property2 = ((Or)property).RightOperand;

                parseStateFormula(property1, out state1, ref is_ctl);
                parseStateFormula(property2, out state2, ref is_ctl);

                result = new SOr(state1, state2);

            }
            else if (property.GetType() == typeof(Not))
            {
                property1 = ((Not)property).Operand;

                parseStateFormula(property1, out state1, ref is_ctl);

                result = new SNot(state1);

            }
            else if (property.GetType() == typeof(Exists))
            {
                property1 = ((Exists)property).Operand;
                parseTemporalOperator(property1, out state1, out state2, ref is_ctl, out temp);

                switch (temp)
                {
                    case 0:
                        result = new SENext(state1);
                        break;
                    case 1:
                        result = new SEUntil(state1, state2);
                        break;
                    case 2:
                        result = new SEFinally(state1);
                        break;
                    case 3:
                        result = new SEAlways(state1);
                        break;
                    case 4:
                        result = new SEWeakUntil(state1, state2);
                        break;
                    default:
                        result = new SError();
                        is_ctl = false;
                        break;        
                }

            }
            else if (property.GetType() == typeof(ForAll))
            {
                property1 = ((ForAll)property).Operand;
                parseTemporalOperator(property1, out state1, out state2, ref is_ctl, out temp);

                switch (temp)
                {
                    case 0:
                        result = new SANext(state1);
                        break;
                    case 1:
                        result = new SAUntil(state1, state2);
                        break;
                    case 2:
                        result = new SAFinally(state1);
                        break;
                    case 3:
                        result = new SAAlways(state1);
                        break;
                    case 4:
                        result = new SAWeakUntil(state1, state2);
                        break;
                    default:
                        result = new SError();
                        is_ctl = false;
                        break;
                }

            } 
            else {
                is_ctl = false;
                result = new SError();
            }

            
        }

        public void parseTemporalOperator(Property property, out StateFormula state1, out StateFormula state2, ref bool is_ctl, out int temp) {

            Property property1, property2;

            if (property.GetType() == typeof(Next))
            {
                property1 = ((Next)property).Operand;
                
                parseStateFormula(property1, out state1, ref is_ctl);
                state2 = new SError();
                temp = 0;

            }
            else if (property.GetType() == typeof(Until))
            {
                property1 = ((Until) property).LeftOperand;
                property2 = ((Until)property).RightOperand;

                parseStateFormula(property1, out state1, ref is_ctl);
                parseStateFormula(property2, out state2, ref is_ctl);
    
                temp = 1;
            }
            else if (property.GetType() == typeof(Eventually)) 
            {
                property1 = ((Eventually)property).Operand;

                parseStateFormula(property1, out state1, ref is_ctl);
                state2 = new SError();
                temp = 2;
            }
            else if (property.GetType() == typeof(Always))
            {
                property1 = ((Always)property).Operand;

                parseStateFormula(property1, out state1, ref is_ctl);
                state2 = new SError();
                temp = 3;
            }
            else if (property.GetType() == typeof(WeakUntil))
            {
                property1 = ((WeakUntil)property).LeftOperand;
                property2 = ((WeakUntil)property).RightOperand;

                parseStateFormula(property1, out state1, ref is_ctl);
                parseStateFormula(property2, out state2, ref is_ctl);

                temp = 4;
            }
            else 
            {
                state1 = new SError();
                state2 = new SError();

                temp = -1;
                is_ctl = false;
            }
            
        }

        public void ModelChecker<T>(TransitionSystem<T> transition_system, LinkedList<T> states, StateFormula state_formula, out bool isSatiesfied)
            where T : struct, Modest.Exploration.IState<T>
        {
            HashSet<T> sat;
            state_formula.isSatiesfied<T>(transition_system, states, out sat);

            T initialState;
            transition_system.GetInitialState(out initialState);

            if (sat.Contains(initialState))
            {
                isSatiesfied = true;
            }
            else
            {
                isSatiesfied = false;
            }
        }


    }
}
