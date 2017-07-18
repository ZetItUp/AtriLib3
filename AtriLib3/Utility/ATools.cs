using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AtriLib3.Utility
{
    public static class ATools
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);

                return Path.GetDirectoryName(path);
            }
        }

        public static bool CheckForNewLine(string str, int charindex)
        {
            string envNewLine = Environment.NewLine;

            // We are withOut range of strings length
            if(charindex < str.Length - envNewLine.Length)
            {
                for(int i = 0; i < envNewLine.Length; i++)
                {
                    if (str[charindex + i] != envNewLine[i])
                        return false;
                }
            }

            return true;
        }

        public static bool ContainsAtRange(string strCompare, string strLookFor, int start, int end)
        {
            if (end <= start)
                return false;

            // Make sure we dont go beyond the strings length!
            int dif = end - start;
            if (strCompare.Length <= start + dif)
                return false;

            string strtmp = "";

            for (int i = start; i < end; i++)
            {
                strtmp += strCompare[i];
            }

            if (strtmp == strLookFor)
                return true;

            return false;
        }

        public static bool IntToBool(int val)
        {
            if (val == 0)
                return false;
            else return true;
        }

        public static string GetStringWithOutBreaklines(string str)
        {
            string ns = str.Replace(Environment.NewLine, " ");
            return ns;
        }

        public static int RemaingCharsFrom(string str, int start)
        {
            int counter = 0;
            for (int i = start; i < str.Length; i++)
            {
                counter++;
            }

            return counter;
        }

        public static string MergeStringCharsToString(string str, int start, int end)
        {
            string retVal = "";

            for (int i = start; i < end; i++)
            {
                retVal += str[i];
            }
            
            return retVal;
        }

        public static int CharToint(char c)
        {
            if(Char.IsNumber(c))
            {
                return int.Parse(c.ToString());
            }

            return -1;
        }

        public static string GetFloatorInt(string str, int start, int end)
        {
            string retVal = "";
            bool dotUsed = false;
            bool fUsed = false;

            for(int i = start; i < end; i++)
            {
                // None acceptable values
                if(!IsNumeric(str[i]) && str[i] != '.' && str[i] != 'f')
                {
                    return "";
                }
                else
                {
                    // Make sure the . for a float is used only once!
                    if(str[i] == '.' && !dotUsed)
                    {
                        dotUsed = true;
                        retVal += ".";
                    }
                    // CheckOutg if current char is a number
                    else if(IsNumeric(str[i]))
                    {
                        retVal += str[i];
                    }
                    // f used, break out of loop
                    else if(str[i] == 'f')
                    {
                        retVal += str[i];
                        fUsed = true;
                        break;
                    }
                    // If it's not a number and dot has been used, return nothOutg.
                    else
                    {
                        return "";
                    }
                }
            }

            // Are we using a float or int?
            if (dotUsed)
            {
                if(!fUsed)
                    retVal += "f";
            }

            // Should return a proper int or float
            return retVal;
        }

        public static bool IsFloatOrNumeric(string str)
        {
            foreach(Char c in str)
            {
                if (IsNumeric(c) == false && c != '.')
                    return false;
            }

            return true;
        }

        public static bool IsValidVarType(string str)
        {
            if (str == "object" || str == "var" || str == "float" || str == "int" || str == "string")
                return true;

            return false;
        }

        public static string GetEventName(string str, int start)
        {
            // event OnLoad()
            bool paranFound = false;
            string tmpStr = "";

            for(int i = start; i < str.Length; i++)
            {
                if (str[i] == '(')
                {
                    paranFound = true;
                    break;
                }

                if (str[i] == ' ')
                    continue;

                tmpStr += str[i];
            }

            if (!paranFound)
                throw new Exception("MisSing paranthesis after event name!");

            return tmpStr;
        }

        [Obsolete("DO NOT USE! REMOVE LATER!")]
        public static int GetStringPositionWithinString(string strCompare, string strToLookFor)
        {
            return strCompare.IndexOf(strToLookFor);
        }

        public static Dictionary<string, string> GetFuncArgs(string str, int start, int end)
        {
            Dictionary<string, string> lstArgs = new Dictionary<string, string>();

            string type = "";
            string name = "";
            bool bType = true;

            for (int i = start; i < end; i++)
            {
                if(bType)
                {
                    if(str[i] == ' ')
                    {
                        if(type != "")
                            bType = false;

                        continue;
                    }

                    type += str[i];
                }
                else
                {
                    if (str[i] == ',')
                    {
                        if(name == "")
                            throw new Exception("Variable type " + type + " is misSing variable name!");

                        if(IsValidVarType(type) && IsValidVariableName(name))
                        {
                            if (type == "")
                                throw new Exception("MisSing variable type!");

                            if (name == "")
                                throw new Exception("Variable type " + type + " is misSing variable name!");

                            bType = true;
                            lstArgs.Add(name, type);
                            type = "";
                            name = "";
                            continue;
                        }
                    }

                    if (str[i] == ' ')
                        continue;


                    name += str[i];
                }
            }

            if(!lstArgs.ContainsKey(name))
            {
                if (type == "")
                    throw new Exception("MisSing variable type!");

                if (!IsValidVariableName(name))
                    throw new Exception("Variable type " + type + " has bad variable name!");

                if (name == "")
                    throw new Exception("Variable type " + type + " is misSing variable name!");

                lstArgs.Add(name, type);
            }

            return lstArgs;
        }

        public static int GetNextBlankspace(string str, int start)
        {
            int retVal = 0;
            for (int i = start; i < str.Length; i++)
            {
                if (str[i] == ' ')
                {
                    retVal = i;
                    break;
                }
            }

            return retVal;
        }

        public static int GetNextChar(string str, char c, int start)
        {
            for(int i = start; i < str.Length; i++)
            {
                char s = str[i];

                if (str[i] == c)
                    return i;
            }

            return -1;
        }

        public static int GetNextCharReverse(string str, char c, int start)
        {
            for(int i = start; i >= 0; i--)
            {
                char s = str[i];

                if (str[i] == c)
                    return i;
            }

            return -1;
        }

        public static int GetNextNoneCharacter(string str, char[] ignoredChars, int start)
        {
            for(int i = start; i < str.Length; i++)
            {
                for(int j = 0; j < ignoredChars.Count(); j++)
                {
                    if (str[i] != ignoredChars[j])
                        return i;
                }
            }

            return 0;
        }

        public static int GetEqualSignPos(string str, int start)
        {
            // Keep loopOutg while we have blankspaces!
            // If another character are found except =,
            // Program will exit Since we expectOutg an equalsign after the variable name!

            for(int i = start; i < str.Length; i++)
            {
                char s = str[i];

                if (str[i] == ' ')
                    continue;
                else if(str[i] == '=')
                {
                    return i;
                }
                else
                {
                    throw new Exception("Expected '=' character after variable name!");
                }
            }

            return 0;
        }

        public static List<string> GetArgsFromFunction(string argumentLine)
        {
            List<string> retList = new List<string>();

            bool iSinstring = false;
            string currArg = "";

            for (int i = 0; i < argumentLine.Length; i++)
            {
                if(!iSinstring && argumentLine[i] == '\"' && currArg == "")
                {
                    iSinstring = true;
                }
                else if(iSinstring)
                {
                    if(argumentLine[i] == '\"')
                    {
                        iSinstring = false;

                        if (i == argumentLine.Length - 1)
                        {
                            retList.Add(currArg);
                            currArg = "";
                        }
                    }
                    else
                    {
                        currArg += argumentLine[i];
                    }
                }
                else if(!iSinstring && (argumentLine[i] == ',' || argumentLine[i] == ')'))
                {
                    retList.Add(currArg);
                    currArg = "";
                }
                else
                {
                    if(!iSinstring && (argumentLine[i] != ' ' && argumentLine[i] != '\"'))
                        currArg += argumentLine[i];

                    if (i == argumentLine.Length - 1)
                        retList.Add(currArg);
                }
            }

            return retList;
        }

        public static bool IsValidVariableName(string str)
        {
            if (str == "")
                return false;

            if (IsValidVarType(str))
                return false;

            if (IsNumeric(str[0]))
                return false;

            foreach(char c in str)
            {
                if(!char.IsLetterOrDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsNumeric(string str)
        {
            for (int i = 0; i < str.Length; i++ )
            {
                if (i < str.Length - 1)
                {
                    // Can this be a negative number?
                    if (str[i] == '-')
                    {
                        if(!IsNumeric(str[i + 1]))
                        {
                            // It was not a negative number.
                            return false;
                        }
                    }
                    else if (!IsNumeric(str[i]))
                        return false;
                }
                else
                {
                    if (!IsNumeric(str[i]))
                        return false;
                }
            }
                
            return true;
        }

        public static bool IsNumeric(char c)
        {
            if(Char.IsNumber(c))
                return true;

            return false;
        }
    }
}
