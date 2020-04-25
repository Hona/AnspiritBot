using System;
using System.Collections.Generic;
using System.Linq;

namespace AnspiritConsoleUI.Utilities
{
    public static class DiscordMessageUtilities
    {
        private static int CodeBlockPrefixOrSuffixLength => CodeBlockPrefix.Length;
        private static string CodeBlockPrefix => $"```{Environment.NewLine}";
        private static string CodeBlockSuffix => $"{Environment.NewLine}```";
        public static string[] GetSendableMessages(string input, bool codeBlock = false)
        {
            if (input.Length < 2000)
            {
                if (codeBlock == true)
                {
                    return new string[] { CodeBlockPrefix + input + CodeBlockSuffix };
                }
                else
                {
                    return new string[] { input };
                }
            }
            else
            {
                var outputLines = new List<string>();
                do
                {
                    var newLineIndex = -1;
                    var indexStartSubtractor = codeBlock ? 2 * CodeBlockPrefixOrSuffixLength : 0;
                    for (int i = 1999 - indexStartSubtractor; i > 0; i--)
                    {
                        var searchedString = input.Substring(i, Environment.NewLine.Length);
                        if (searchedString == Environment.NewLine)
                        {
                            newLineIndex = i;
                            break;
                        }
                    }

                    if (newLineIndex == -1)
                    {
                        // No newlines, just split it at the end
                        newLineIndex = 1999 - indexStartSubtractor;
                    }
                    if (codeBlock)
                    {
                        outputLines.Add(CodeBlockPrefix + new string(input.Take(newLineIndex).ToArray()) + CodeBlockSuffix);
                    }
                    else
                    {
                        outputLines.Add(new string(input.Take(newLineIndex).ToArray()));
                    }
                    input = new string(input.Skip(newLineIndex).ToArray());
                } while (input.Length >= 2000);
                if (codeBlock)
                {
                    outputLines.Add(CodeBlockPrefix + input + CodeBlockSuffix);
                }
                else
                {
                    outputLines.Add(input);
                }
                return outputLines.ToArray();

            }
        }
    }
}
