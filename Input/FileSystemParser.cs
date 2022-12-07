using Directory = AdventOfCode2022.Models.Directory;
using File = AdventOfCode2022.Models.File;

namespace AdventOfCode2022.Input
{
    public class FileSystemParser : IInputParser<Directory>
    {
        public Directory ParseInput(string input)
        {
            var lines = input.Split(Environment.NewLine);
            return Parse(lines);
        }

        private static string ChangeDirectory(string currentDir, string arg)
        {
            switch (arg)
            {
                case "..":
                    var s = currentDir.Split("/", StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length == 0)
                        return "/";
                    return "/" + string.Join("/", s.Take(s.Length - 1));
                case "/":
                    return "/";
                default:
                    return currentDir.EndsWith('/') ? currentDir + arg : currentDir + "/" + arg;
            }
        }

        private static Directory Parse(string[] input)
        {
            string currentDir = "/";
            var directories = new Dictionary<string, Directory>();
            directories.Add("/", new Directory("/"));
            int cl = 0;
            while(cl < input.Length)
            {
                var command = input[cl++];
                var split = command.Split(' ');
                switch (split[1])
                {
                    case "cd":
                        currentDir = ChangeDirectory(currentDir, split[2]);
                        break;
                    case "ls":
                        var dir = GetDir(currentDir, directories);
                        dir.Files.Clear();
                        dir.Directories.Clear();
                        while (cl < input.Length && !input[cl].StartsWith("$"))
                        {
                            var s = input[cl].Split();
                            if (s[0] == "dir")
                            {
                                var path = ChangeDirectory(currentDir, s[1]);
                                var childDir = GetDir(path, directories);
                                dir.Directories.Add(childDir);
                            }
                            else
                            {
                                dir.Files.Add(new File(s[1], int.Parse(s[0])));
                            }
                            cl++;
                        }
                        break;
                }
            }
            return directories["/"];
        }

        private static Directory GetDir(string path, Dictionary<string, Directory> directories)
        {
            if (!directories.TryGetValue(path, out var d))
            {
                d = new Directory(path);
                directories.Add(path, d);
            }
            return d;
        }
    }
}