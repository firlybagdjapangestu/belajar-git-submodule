using System;
using System.Text.Json;

namespace BelajarGitSubmodule
{
    public class Release
    {
        public string Version { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }

        public Release() { }

        public Release(string version, string notes = "", DateTime? date = null)
        {
            Version = version;
            Notes = notes;
            Date = date ?? DateTime.UtcNow;
        }

        public override string ToString() =>
            $"{Version} - {Date:yyyy-MM-dd} - {Notes}";

        public Release BumpPatch()
        {
            var (maj, min, patch) = ParseSemVer(Version);
            patch++;
            return new Release($"{maj}.{min}.{patch}", Notes, DateTime.UtcNow);
        }

        public Release BumpMinor()
        {
            var (maj, min, _) = ParseSemVer(Version);
            min++;
            return new Release($"{maj}.{min}.0", Notes, DateTime.UtcNow);
        }

        public Release BumpMajor()
        {
            var (maj, _, _) = ParseSemVer(Version);
            maj++;
            return new Release($"{maj}.0.0", Notes, DateTime.UtcNow);
        }

        (int maj, int min, int patch) ParseSemVer(string v)
        {
            var parts = (v ?? "0.0.0").Split('.', StringSplitOptions.RemoveEmptyEntries);
            int p0 = parts.Length > 0 && int.TryParse(parts[0], out var a) ? a : 0;
            int p1 = parts.Length > 1 && int.TryParse(parts[1], out var b) ? b : 0;
            int p2 = parts.Length > 2 && int.TryParse(parts[2], out var c) ? c : 0;
            return (p0, p1, p2);
        }

        public string ToJson() => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        public static Release FromJson(string json) => JsonSerializer.Deserialize<Release>(json) ?? new Release("0.0.0");
    }

    // Simple demo when run as an application
    class Program
    {
        static void Main()
        {
            var r = new Release("1.2.3", "Initial release");
            Console.WriteLine("Current: " + r);

            var patch = r.BumpPatch();
            Console.WriteLine("Patch bumped: " + patch);

            var minor = r.BumpMinor();
            Console.WriteLine("Minor bumped: " + minor);

            var major = r.BumpMajor();
            Console.WriteLine("Major bumped: " + major);

            var json = r.ToJson();
            Console.WriteLine("JSON:\n" + json);

            var fromJson = Release.FromJson(json);
            Console.WriteLine("From JSON: " + fromJson);
        }
    }
}