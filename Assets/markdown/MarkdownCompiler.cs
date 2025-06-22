using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Assets.model;


namespace Assets.markdown
{
    public static class MarkdownCompiler
    {

        public static InstructionDTO Compile(string markdown)
        {
            var lines = markdown.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            string shortTitle = null;
            string title = null;
            int id = -1;
            var tasks = new List<(string, string)>();

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();

                if (line.StartsWith("# ") && shortTitle == null)
                    shortTitle = line.Substring(2).Trim();
                else if (line.StartsWith("## ") && title == null)
                    title = line.Substring(3).Trim();
                else if (line.StartsWith("### ") && id == -1)
                    int.TryParse(line.Substring(4).Trim(), out id);
                else if (Regex.IsMatch(line, @"^\d+\.\s"))
                {
                    Match match = Regex.Match(line, @"^(\d+)\.\s");

                    if (match.Success)
                    {
                        int taskNumber = int.Parse(match.Groups[1].Value);

                        if (taskNumber != tasks.Count + 1)
                        {
                            throw new Exception($"Mismatch between compiled task and Markdown List index\nExpected {tasks.Count + 1}(compiled), got {taskNumber}(in file)");
                        }
                    }

                    string taskTitle = Regex.Replace(line, @"^\d+\.\s*", "").Trim();
                    if (i + 1 < lines.Length)
                    {
                        string taskDescription = lines[i + 1].Trim();

                        if (string.IsNullOrWhiteSpace(taskTitle))
                        {
                            throw new Exception($"Could not get the title of the task\nLine {i}: {line}");
                        }

                        tasks.Add((taskTitle, taskDescription));
                        i++;
                    }
                }
            }

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(shortTitle) || id == -1)
            {
                throw new Exception("Malformed format, unable to compile");
            }

            var dto = new Assets.model.InstructionDTO(shortTitle, title) { id = id };
            foreach (var task in tasks)
                dto.addTask(task.Item1, task.Item2);

            if (dto.tasks.Count == 0)
            {
                throw new Exception("Malformed format, checklist without tasks!");
            }

            return dto;
        }
    }
}
