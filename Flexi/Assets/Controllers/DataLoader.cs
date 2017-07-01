using System;
using System.Collections.Generic;
using System.IO;
using Flexi.Assets.Models;

namespace Flexi.Assets.Controllers
{
    public class DataLoader
    {
        public List<Day> ReadAllFiles()
        {
            var dayList = new List<Day>();

            try
            {
                var appPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var fullPath = Path.Combine(appPath, "Flexi");
                var file = fullPath + "/Entries.dat";

                var reader = new StreamReader(file);

                var line = reader.ReadLine();

                while (line != null)
                {
                    var split = line.Split('@');
                    var startStr = split[0];
                    var endStr = split[1];
                    var doneStr = split[2];

                    var done = bool.Parse(doneStr);
                    var start = DateTime.Parse(startStr);
                    var end = DateTime.Parse(endStr);

                    dayList.Add(new Day(start, end, done));

                    line = reader.ReadLine();
                }

                reader.Close();

            }
            catch (FileNotFoundException){}
            catch (DirectoryNotFoundException){}
            
            return dayList;
        }
    }
}