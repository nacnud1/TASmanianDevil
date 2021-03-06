﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamoServices;

namespace H72File
{
    /// <summary>
    /// H72 file node
    /// </summary>
    public class H72Node
    {
        private static List<Tuple<bool, string, H72Node>> pH72NodeListCollector;

        private string pName;
        private List<H72Node> pH72NodeList;
        private List<H72Parameter> pH72ParameterList;

        internal H72Node(string FilePath)
        {
            pH72NodeList = new List<H72Node>();

            try
            {
                System.IO.Path.Combine(FilePath);
                pName = System.IO.Path.GetFileName(FilePath); ;
            }
            catch
            {
                pName = GetType().ToString();
            }

            if (System.IO.File.Exists(FilePath))
            {
                string[] aLines = System.IO.File.ReadAllLines(FilePath, Encoding.Default);
                int aIndex = 0;
                while (aIndex < aLines.Length)
                {
                    if (aLines[aIndex].Trim().StartsWith("["))
                    {
                        int aEndIndex = aIndex;
                        H72Node aH72Node = new H72Node(aLines, aIndex, out aEndIndex);
                        if (aH72Node.Name != null)
                            pH72NodeList.Add(aH72Node);
                        aIndex = aEndIndex;
                    }
                    else
                        aIndex++;
                }
            }

        }

        internal H72Node(string[] Lines, int StartIndex, out int EndIndex)
        {
            EndIndex = StartIndex + 1;
            pName = null;
            pH72NodeList = null;
            pH72ParameterList = null;
            if (Lines.Length > StartIndex)
            {
                string aLine = Lines[StartIndex];
                string aTrimmedLine = aLine.TrimStart();
                if (aTrimmedLine.StartsWith("["))
                {
                    pH72NodeList = new List<H72Node>();
                    pH72ParameterList = new List<H72Parameter>();
                    pName = aTrimmedLine.TrimEnd();
                    if (pName.Length > 1)
                        pName = pName.Substring(1, pName.Length - 2);
                    int aNestingIndex = aLine.Length - aTrimmedLine.Length;
                    EndIndex = StartIndex + 1;
                    while (EndIndex < Lines.Length)
                    {
                        aLine = Lines[EndIndex];
                        if (!string.IsNullOrEmpty(aLine) || !string.IsNullOrWhiteSpace(aLine))
                        {
                            aTrimmedLine = aLine.TrimStart();
                            int aCurrentNestingIndex = aLine.Length - aTrimmedLine.Length;

                            if (aCurrentNestingIndex < aNestingIndex)
                                break;

                            if (aTrimmedLine.StartsWith("["))
                            {
                                if (aCurrentNestingIndex == aNestingIndex)
                                    break;

                                int aNewEndIndex = 0;
                                H72Node aH72Node = new H72Node(Lines, EndIndex, out aNewEndIndex);
                                if (aH72Node.pName != null)
                                    pH72NodeList.Add(aH72Node);
                                EndIndex = aNewEndIndex - 1;
                            }
                            else if (aTrimmedLine.Contains("="))
                            {
                                aTrimmedLine = aTrimmedLine.TrimEnd();
                                int aIndex = aTrimmedLine.IndexOf('=');
                                if (aIndex > 0)
                                {
                                    H72Parameter aH72Parameter = new H72Parameter(aTrimmedLine.Substring(0, aIndex), aTrimmedLine.Substring(aIndex + 1));
                                    pH72ParameterList.Add(aH72Parameter);
                                }
                            }
                            else if(aTrimmedLine.StartsWith("*"))
                            {
                                EndIndex++;
                                if(EndIndex < Lines.Length)
                                {
                                    aTrimmedLine = aTrimmedLine.TrimEnd();
                                    H72Parameter aH72Parameter = new H72Parameter(aTrimmedLine, Lines[EndIndex].Trim());
                                    pH72ParameterList.Add(aH72Parameter);
                                }
                            }
                        }
                        EndIndex++;

                    }
                }
            }


        }

        internal void AppendStringList(int Index, List<string> StringList)
        {
            string aPrefix = new string(' ', Index*3);
            StringList.Add(string.Format("{0}[{1}]", aPrefix, pName));
            if (pH72ParameterList != null)
                foreach (H72Parameter aH72Parameter in pH72ParameterList)
                    aH72Parameter.AppendStringList(Index, StringList);

            if (pH72NodeList != null)
                foreach (H72Node aH72Node in pH72NodeList)
                {
                    StringList.Add(string.Empty);
                    aH72Node.AppendStringList(Index + 1, StringList);
                }
        }

        internal string Name
        {
            get
            {
                return pName;
            }
        }

        private void Save(object FilePath)
        {
            List<string> aStringList = new List<string>();
            foreach (H72Node aH72Node in pH72NodeList)
            {
                aH72Node.AppendStringList(0, aStringList);
                aStringList.Add(string.Empty);
            }
            if (aStringList.Count > 0)
                aStringList.RemoveAt(aStringList.Count - 1);
            System.IO.File.WriteAllLines(FilePath.ToString(), aStringList, Encoding.Default);
        }

        private static void ExecutionEvents_GraphPostExecution(Dynamo.Session.IExecutionSession IExecutionSession)
        {
            //Dynamo.Events.ExecutionEvents.GraphPostExecution -= ExecutionEvents_GraphPostExecution;
            if(pH72NodeListCollector != null)
            {
                pH72NodeListCollector.RemoveAll(x => !x.Item1);
                foreach (Tuple<bool, string, H72Node> aTuple in pH72NodeListCollector)
                    aTuple.Item3.Save(aTuple.Item2);
                pH72NodeListCollector = null;
            }
        }

        /// <summary>
        /// Gets all H72 parameters
        /// </summary>
        /// <param name="H72Node">TAS H72 Node</param>
        /// <returns name="Parameters">H72 Parameter List</returns>
        /// <search>
        /// TAS, H72, H72Node, H72 Node, GetParameters, Get Parameters, getparameters, get parameters
        /// </search>
        public static List<H72Parameter> GetParameters(H72Node H72Node)
        {
            return H72Node.pH72ParameterList;
        }

        /// <summary>
        /// Gets H72 Parameter by given name
        /// </summary>
        /// <param name="H72Node">TAS H72 Node</param>
        /// <param name="Name">Parameter Name</param>
        /// <returns name="Parameter">H72 Parameter</returns>
        /// <search>
        /// TAS, H72, H72Node, H72 Node, GetParameter, Get Parameter, getparameter, get parameter
        /// </search>
        public static H72Parameter GetParameterByName(H72Node H72Node, string Name)
        {
            return H72Node.pH72ParameterList.Find(x => x.Name == Name);
        }

        /// <summary>
        /// Gets H72 Parameter Value by given name
        /// </summary>
        /// <param name="H72Node">TAS H72 Node</param>
        /// <param name="Name">Parameter Name</param>
        /// <returns name="Value">Parameter value</returns>
        /// <search>
        /// TAS, H72, H72Node, H72 Node, GetParameterValueByName, Get Parameter Value By Name, getparametervaluebyname, get parameter value by name
        /// </search>
        public static object GetParameterValueByName(H72Node H72Node, string Name)
        {
            H72Parameter aH72Parameter = H72Node.pH72ParameterList.Find(x => x.Name == Name);
            if (aH72Parameter != null)
                return aH72Parameter.Value;
            else
                return null;
        }

        /// <summary>
        /// Sets H72 Parameter for Node
        /// </summary>
        /// <param name="H72Node">TAS H72 Node</param>
        /// <param name="Name">Parameter Name</param>
        /// <param name="Value">Parameter Value</param>
        /// <returns name="H72Parameter">Parameter</returns>
        /// <search>
        /// TAS, H72, H72Node, H72 Node, SetParameter, Set Parameter, set parameter, setparameter, seth72parameter,
        /// </search>
        public static H72Parameter SetParameter(H72Node H72Node, string Name, object Value)
        {
            H72Parameter aH72Parameter = H72Node.pH72ParameterList.Find(x => x.Name == Name);
            if(aH72Parameter == null)
            {
                aH72Parameter = new H72Parameter(Name);
                H72Node.pH72ParameterList.Add(aH72Parameter);
            }
            aH72Parameter.Value = Value;

            return aH72Parameter;
        }

        /// <summary>
        /// Removes H72 Parameter for Node
        /// </summary>
        /// <param name="H72Node">TAS H72 Node</param>
        /// <param name="Name">Parameter Name</param>
        /// <returns name="Removed">Removed</returns>
        /// <search>
        /// TAS, H72, H72Node, H72 Node, RemoveParameter, Remove Parameter, remove parameter, removeparameter, removeh72parameter,
        /// </search>
        public static bool RemoveParameter(H72Node H72Node, string Name)
        {
            int aCount = H72Node.pH72ParameterList.Count;
            H72Node.pH72ParameterList.RemoveAll(x => x.Name == Name);

            return aCount != H72Node.pH72ParameterList.Count;
        }

        /// <summary>
        /// Gets H72 node name
        /// </summary>
        /// <param name="H72Node">TAS H72 Node</param>
        /// <returns name="Name">H72 Node name</returns>
        /// <search>
        /// TAS, H72, H72Node, H72 Node, GetName, Get Name, getname, get name
        /// </search>
        public static string GetName(H72Node H72Node)
        {
            return H72Node.pName;
        }

        /// <summary>
        /// Gets all child nodes for H72 node
        /// </summary>
        /// <param name="H72Node">TAS H72 Node</param>
        /// <returns name="Nodes">H72 Nodes</returns>
        /// <search>
        /// TAS, H72, H72Node, H72 Node, GetName, Get Name, getname, get name
        /// </search>
        public static List<H72Node> GetNodes(H72Node H72Node)
        {
            return H72Node.pH72NodeList;
        }

        /// <summary>
        /// Gets child nodes by name for H72 node
        /// </summary>
        /// <param name="H72Node">TAS H72 Node</param>
        /// <param name="Name">Node Name</param>
        /// <returns name="Nodes">H72 Nodes</returns>
        /// <search>
        /// TAS, H72, H72Node, H72 Node, GetNodesByName, Get Nodes By Name, getnodesbyname, get nodes by name
        /// </search>
        public static List<H72Node> GetNodesByName(H72Node H72Node, string Name)
        {
            return H72Node.pH72NodeList.FindAll(x => x.pName == Name);
        }

        /// <summary>
        /// Opens TAS H72 File
        /// </summary>
        /// <param name="FilePath">File Path</param>
        /// <param name="Save">Save File</param>
        /// <returns name="H72Node">H72 Node</returns>
        /// <search>
        /// TAS, H72Node, tas, h72node, open h72 node, openh72node
        /// </search>
        public static H72Node Open(object FilePath, bool Save = false)
        {
            Dynamo.Events.ExecutionEvents.GraphPostExecution -= ExecutionEvents_GraphPostExecution;
            Dynamo.Events.ExecutionEvents.GraphPostExecution += ExecutionEvents_GraphPostExecution;

            string aFilePath = FilePath.ToString();
            H72Node aH72Node = new H72Node(aFilePath);

            if (pH72NodeListCollector == null)
                pH72NodeListCollector = new List<Tuple<bool, string, H72Node>>();
            pH72NodeListCollector.Add(new Tuple<bool, string, H72Node>(Save, aFilePath, aH72Node));

            return aH72Node;
        }

        public override string ToString()
        {
            return string.Format("{0}(Name = {1})", "H72Node", pName);
        }
    }
}
