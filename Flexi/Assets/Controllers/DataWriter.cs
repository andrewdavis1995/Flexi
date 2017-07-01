using System.Collections.Generic;
using System.IO;
using Flexi.Assets.Models;

namespace Flexi.Assets.Controllers
{
    class DataWriter
    {
        public void WriteAllToFile(List<Day> dayList)
        {
            var appPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var fullPath = Path.Combine(appPath, "Flexi");
            var file = fullPath + "/Entries.dat";

            var writer = new StreamWriter(file, false);

            foreach (var day in dayList)
            {
                writer.WriteLine(day.StartTime + "@" + day.EndTime + "@" + day.Done);
            }

            writer.Close();
        }
    }
}