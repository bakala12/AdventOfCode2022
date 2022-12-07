namespace AdventOfCode2022.Models
{
    public record class File(string Name, int Size);

    public record class Directory(string Path)
    {
        public List<File> Files { get; set; } = new List<File>();
        public List<Directory> Directories { get; set; } = new List<Directory>();

        public long GetSize() => Files.Sum(f => f.Size) + Directories.Sum(d => d.GetSize());
    }
}