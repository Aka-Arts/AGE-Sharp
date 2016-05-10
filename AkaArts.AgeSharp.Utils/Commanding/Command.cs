using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkaArts.AgeSharp.Utils.Commanding
{
    public class Command
    {
        public const Char PART_SEPARATOR = ' ';
        public const Char SEPARATOR_ESCAPE = '"';
        public readonly String Instruction;
        public readonly List<String> Arguments;
        public readonly String Raw;

        private Command(String instruction, List<String> arguments, String raw)
        {
            this.Instruction = instruction;
            this.Arguments = arguments;
            this.Raw = raw;
        }

        public static Command Create(String command)
        {
            var commandParts = Parse(command);

            if (commandParts != null && commandParts.Count > 0)
            {
                var instruction = commandParts[0];
                var arguments = new List<String>();
                for (int i = 1 ; i < commandParts.Count ; i++)
                {
                    arguments.Add(commandParts[i]);
                }
                return new Command(instruction, arguments, command.Trim());
            }
            // otherwise null
            return null;
        }

        private static List<String> Parse(String command)
        {
            command = command.Trim();

            var parts = new List<String>();
            var length = command.Length;

            if (length < 1)
            {
                return null;
            }

            var instructionDone = false;
            var quotesOpen = false;
            var partStart = 0;

            for (int i = 0 ; i < length ; i++)
            {
                var currentCharacter = command[i];

                if (!instructionDone && currentCharacter == SEPARATOR_ESCAPE)
                {
                    // return null if escape comes in struction part
                    return null;
                }

                if (!instructionDone)
                {
                    // check for instruction end (by separator or end of string)
                    if (currentCharacter == PART_SEPARATOR)
                    {
                        var instruction = command.Substring(partStart, i - partStart);
                        parts.Add(instruction.ToLowerInvariant());
                        instructionDone = true;
                    }
                    else if (i + 1 >= length)
                    {
                        var instruction = command.Substring(partStart, (i + 1) - partStart);
                        parts.Add(instruction.ToLowerInvariant());
                        instructionDone = true;
                    }
                }
                else
                {
                    // check for arguments
                    if (quotesOpen)
                    {
                        if (currentCharacter == SEPARATOR_ESCAPE)
                        {
                            // may be end of block or escaped "" => ", look ahead
                            if (i + 1 >= length || command[i + 1] != SEPARATOR_ESCAPE)
                            {
                                // end of block
                                var argument = command.Substring(partStart, i - partStart);
                                parts.Add(argument);
                                quotesOpen = false;
                            }
                        }
                    }
                    else
                    {
                        if (command[i - 1] == PART_SEPARATOR && currentCharacter != PART_SEPARATOR)
                        {
                            // start of a new part
                            if (currentCharacter == SEPARATOR_ESCAPE)
                            {
                                quotesOpen = true;
                                partStart = i + 1;
                            }
                            else
                            {
                                partStart = i;
                            }
                        }
                        else if (command[i - 1] != PART_SEPARATOR && command[i - 1] != SEPARATOR_ESCAPE && currentCharacter == PART_SEPARATOR)
                        {
                            var argument = command.Substring(partStart, i - partStart);
                            parts.Add(argument);
                        }

                        if (i + 1 >= length)
                        {
                            var argument = command.Substring(partStart, i + 1 - partStart);
                            parts.Add(argument);
                        }
                    }
                }
            }

            if (quotesOpen)
            {
                // were never closed
                return null;
            }

            return parts;
        }
    }
}
