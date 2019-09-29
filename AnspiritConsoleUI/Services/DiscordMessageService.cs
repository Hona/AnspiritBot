using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnspiritConsoleUI.Services
{
    public static class DiscordMessageService
    {
        private static int CodeBlockPrefixOrSuffixLength => CodeBlockPrefix.Length;
        private static string CodeBlockPrefix => $"```{Environment.NewLine}";
        private static string CodeBlockSuffix => $"{Environment.NewLine}```";
        public static string[] GetSendableCodeblockMessages(string input)
        {
            if (input.Length < 2000)
            {
                return new string[] {CodeBlockPrefix + input + CodeBlockSuffix};
            }
            else
            {
                var outputLines = new List<string>();
                do
                {
                    var newLineIndex = -1;
                    for (int i = 1999 - 2 * CodeBlockPrefixOrSuffixLength; i > 0; i--)
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
                        newLineIndex = 1999 - 2 * CodeBlockPrefixOrSuffixLength;
                    }

                    outputLines.Add(CodeBlockPrefix + new string(input.Take(newLineIndex).ToArray()) + CodeBlockSuffix);
                    input = new string(input.Skip(newLineIndex).ToArray());
                } while (input.Length >= 2000);
                outputLines.Add(CodeBlockPrefix + input + CodeBlockSuffix);
                return outputLines.ToArray();

            }
        }
    }
}
