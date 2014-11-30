using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modest.Teaching;
using CI = System.Globalization.CultureInfo;

namespace TransitionSystemChecker
{
    class TestAnalyzer
    {
        public TestAnalyzer()
        {

        }

        public void writeTestFiles<T>(TransitionSystem<T> transitionSystem, ModelProperty[] properties)
            where T : struct, Modest.Exploration.IState<T>
        {
            String text1 = "TransitionSystem: \n";
            String text2 = transitionSystem.ToString() + " \n" + " \n" + " \n";
            String text3 = "Properties: \n";
            String text4 = "";

            for (int i = 0; i < properties.Count(); i++)
            {

                Property property = properties[i].Property;
                String name = properties[i].Name;
                String prop = property.ToString();
                String loc = property.Location.ToString();

                text4 = text4 + "Name: " + name + " \n" + prop + " Location: " + loc +   " \n";
            }

            String text = text1 + text2 + text3 + text4 ;

            String nextFileName = getNextFileName("property_location");
            System.IO.File.WriteAllText(nextFileName, text);


        }

        public void Program_Analyze_Tests(String fileName)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(fileName);

            String line;
            String text = "";

            try
            {
                while ((line = file.ReadLine()) != null)
                {
                    text = text + line + " \n";
                }

                file.Close();
            }
            catch (Exception e)
            {
                // Do something maybe
            }

            String nextFileName = getNextFileName("test");
            System.IO.File.WriteAllText(nextFileName, text);

        }

        public String getNextFileName(String name)
        {
             int fileNameCounter = 0;

            System.IO.StreamReader file =
                new System.IO.StreamReader("C:\\Users\\Umangathan\\Downloads\\Programmierung\\Verification\\Project_1\\Tests\\" + name + fileNameCounter + ".txt");

            try
            {
                while (file.ReadLine() != null)
                {
                    fileNameCounter++;
                    file = new System.IO.StreamReader("C:\\Users\\Umangathan\\Downloads\\Programmierung\\Verification\\Project_1\\Tests\\" + name + fileNameCounter + ".txt");
                }

                file.Close();
            }
            catch (Exception e)
            {
                // Do something maybe
            }

            String nextFileName = @"C:\Users\Umangathan\Downloads\Programmierung\Verification\Project_1\Tests\" + name + fileNameCounter + ".txt";

          return nextFileName;
        }
    }
}
