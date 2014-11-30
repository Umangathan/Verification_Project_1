using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modest.Teaching;
using TransitionSystemChecker.StateFormulas;
using TransitionSystemChecker.PathFormulas;
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

            // Transform properties in State_Formulas;
            var state_formulas = new Dictionary<String, StateFormula>();

            foreach (var model_property in properties)
            {
                Property property = model_property.Property;
                String name = model_property.Name;
                StateFormula complete_formula = new SError();
                bool is_ctl = true;


                parseStateFormula(property, name, ref complete_formula, ref is_ctl);


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


            //  Environment.Exit(0);

        }

        public void traverseTree<T>(TransitionSystem<T> transitionSystem, ref List<T> oldStates, ref Queue<T> newStates, ref T state_a, ref bool terminalStatesEncountered)
            where T : struct, Modest.Exploration.IState<T>
        {

            //HashSet<T> successors = new HashSet<T>();

            int newSize = newStates.Count;

            //var newestStates = new List<T>();

            while (newSize > 0)
            {
                //Console.WriteLine(newSize);
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
                        if (!(oldStates.Contains(successor)) && !(newStates.Contains(successor)))
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



        public void parseStateFormula(Property property, String name, ref StateFormula state_formula, ref bool is_ctl)
        {
            StateFormula left_state = new SError();
            StateFormula right_state = new SError();

            if (property.GetType() == typeof(True))
            {   
                state_formula = new SBoolean(true);
            }
            else if (property.GetType() == typeof(False))
            {
                state_formula = new SBoolean(false);
            }
            else if (property.GetType() == typeof(AtomicProposition))
            {
                
                state_formula = new SAtomic((AtomicProposition)property);
            }
            else if (property.GetType() == typeof(And))
            {
                Property left = ((And)property).LeftOperand;
                Property right = ((And)property).RightOperand;


                parseStateFormula(left, name, ref left_state, ref is_ctl);
                parseStateFormula(right, name, ref right_state, ref is_ctl);

                state_formula = new SAnd(left_state, right_state);

            }
            else if (property.GetType() == typeof(Or))
            {
                Property left = ((And)property).LeftOperand;
                Property right = ((And)property).RightOperand;

                parseStateFormula(left, name, ref left_state, ref is_ctl);
                parseStateFormula(right, name, ref right_state, ref is_ctl);

                state_formula = new SOr(left_state, right_state);
            }
            else if (property.GetType() == typeof(Not))
            {
                Property operand = ((Not)property).Operand;

                StateFormula not_state = new SError();
                parseStateFormula(operand, name, ref not_state, ref is_ctl);

                state_formula = new SNot(not_state);
            }
            else if (property.GetType() == typeof(Exists))
            {
                Property operand = ((Exists)property).Operand;

                PathFormula path_operand = new PError();
                parsePathFormula(operand, name, ref path_operand, ref is_ctl);

                state_formula = new SExists(path_operand);
            }
            else if (property.GetType() == typeof(ForAll))
            {
                
                Property operand = ((ForAll)property).Operand;

                PathFormula path_operand = new PError();
                parsePathFormula(operand, name, ref path_operand, ref is_ctl);

                state_formula = new SForAll(path_operand);

            }
            else
            {
                is_ctl = false;
            }

            


        }

        public void parsePathFormula(Property property, String name, ref PathFormula path_formula, ref bool is_ctl)
        {
            StateFormula operand_state = new SError();
            StateFormula left_state = new SError();
            StateFormula right_state = new SError();
            if (property.GetType() == typeof(Next))
            {
                Property operand = ((Next)property).Operand;
                parseStateFormula(operand, name, ref operand_state, ref is_ctl);
                path_formula = new PNext(operand_state);
            }
            else if (property.GetType() == typeof(Until))
            {
                Property left = ((Until)property).LeftOperand;
                Property right = ((Until)property).RightOperand;

                parseStateFormula(left, name, ref left_state, ref is_ctl);
                parseStateFormula(right, name, ref right_state, ref is_ctl);

                path_formula = new PUntil(left_state, right_state);

            }
            else if (property.GetType() == typeof(Until))
            {
                Property operand = ((Next)property).Operand;
                parseStateFormula(operand, name, ref operand_state, ref is_ctl);
                path_formula = new PEventually(operand_state);
            }
            else if (property.GetType() == typeof(Always))
            {
                Property operand = ((Always)property).Operand;
                parseStateFormula(operand, name, ref operand_state, ref is_ctl);
                path_formula = new PGlobally(operand_state);
            }
            else if (property.GetType() == typeof(WeakUntil))
            {
                Property left = ((WeakUntil)property).LeftOperand;
                Property right = ((WeakUntil)property).RightOperand;

                parseStateFormula(left, name, ref left_state, ref is_ctl);
                parseStateFormula(right, name, ref right_state, ref is_ctl);

                path_formula = new PWeakUntil(left_state, right_state);
            } else
            {
                is_ctl = false;

            }



        }
    }
}
