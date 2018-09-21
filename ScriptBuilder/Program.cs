﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ScriptBuilder
{

    class Program
    {
        public class Script
        {
            public int ScriptIndex { get; set; }
            public string ScriptValue { get; set; }
        }

        private static List<string> ListScriptDirectories = new List<string>()
        {
            @"C:\Users\firel\Desktop\Script1.txt",
            @"C:\Users\firel\Desktop\Script2.txt",
            @"C:\Users\firel\Desktop\Script3.txt",
        };

        static void Main(string[] args)
        {
            var listScript = new List<Script>();
            int i = 1;
            foreach (var item in ListScriptDirectories)
            {

                using (var stream = new StreamReader(item))
                {
                    string line = stream.ReadLine();

                    listScript.Add(new Script()
                    {
                        ScriptIndex = i,
                        ScriptValue = line
                    });

                }
                i++;
            }
            string result = JsonConvert.SerializeObject(listScript);
            System.IO.File.WriteAllText(@"C:\Users\firel\Desktop\Script.json", result);
        }
    }
}