// Program.cs
// You should not need to modify this file.
//
using System;
using System.IO;
using Modest.Modularity;
using Modest.Teaching;

namespace TransitionSystemChecker
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				// Load necessary Modest components
				Components.Instance.LoadPlugin("Modest.Language.dll");
				Components.Instance.LoadPlugin("Modest.Automaton.dll");
				var modestInput = (IInputToolchain)Components.Instance.GetRequired(new Guid("41F25138-8099-48C4-AB22-A424F1CC3EBF"));

				// Open input file (first parameter)
				if(args.Length < 1)
				{
					Console.Error.WriteLine("No input file specified.");
					return;
				}
				Stream stream = null;
				try
				{
					stream = File.Open(args[0], FileMode.Open, FileAccess.Read, FileShare.Read);
				}
				catch(FileNotFoundException)
				{
					Console.Error.WriteLine("The specified input file does not exist.");
					return;
				}
				catch
				{
					Console.Error.WriteLine("Error opening the input file.");
					return;
				}

				// Load model and hand over to checker
				var errors = new ConsoleErrorHandler();
				try
				{   /*
                    TestAnalyzer test_analyzer = new TestAnalyzer();
                    test_analyzer.Program_Analyze_Tests(args[0]);
                    */


					var model = (Modest.Automaton.NSTAModel)modestInput.Load(modestInput.GetDefaultInputParameters(), new Stream[] { stream }, new string[] { args[0] }, new OperationState(), errors);
					stream.Dispose();
					if(model != null)
					{
						var checker = new Checker();
						if(checker.ParseCommandLine(args)) model.AutomataNetwork.Analyze(checker);
					}
				}
				catch(ErrorException ee)
				{
					errors.Add(ee.ToError(Guid.Empty));
				}
			}
			finally
			{
#if DEBUG
				Console.WriteLine("Press any key to continue.");
				Console.ReadKey();
#endif
			}
		}
	}

	sealed class ConsoleErrorHandler: IErrorHandler
	{
		public void Add(IError error)
		{
			Console.Error.WriteLine(error.ToString());
		}
	}
}
