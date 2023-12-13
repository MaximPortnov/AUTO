using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Documents;

namespace AmonicAirLines
{
    public class Session
    {
        public TimeSpan totalTime;
        public DateTime startTime;
        public DateTime endTime;
        public bool crash;
        public Session()
        {
            totalTime = TimeSpan.Zero;
            startTime = DateTime.Now;
            endTime = DateTime.MinValue;
            crash = true;
        }
        public void endSession()
        {
            endTime = DateTime.Now;
            totalTime = TimeSpan.FromTicks(endTime.Ticks - startTime.Ticks);
        }
        public long getStartTicks() => startTime.Ticks;
        public long getEndTicks() => endTime.Ticks;
    }
    [Serializable]
    


    internal class AppControle
    {
        [NonSerialized] private const string fileName = "AmonicAirlines.bin";
        [NonSerialized] static public AppControle obj = null;

        private TimeSpan totalTime;
        List<Session> sessions = new List<Session>();

        private Session getLastSession() => sessions[sessions.Count - 1];
        private AppControle()
        {
            totalTime = TimeSpan.Zero;
            sessions.Add(new Session());
        }

        private void load()
        {
            sessions.Add(new Session());
        }

        private void save()
        {
            getLastSession().endSession();
            //Console.WriteLine($"start = {getLastSession().startTime}");
            //Console.WriteLine($"end = {getLastSession().endTime}");
            //Console.WriteLine($"total = {getLastSession().endTime - getLastSession().startTime}");
            totalTime += TimeSpan.FromTicks(getLastSession().getEndTicks() - getLastSession().getStartTicks());
        }

        public TimeSpan getNowTotalTime()
        {
            Console.Write($"start = {getLastSession().startTime}\t");
            Console.Write($"now = {DateTime.Now}\t");
            Console.Write($"total = {getLastSession().endTime - getLastSession().startTime}\ttotalTime{totalTime + TimeSpan.FromTicks(DateTime.Now.Ticks - getLastSession().startTime.Ticks)}\n");
            return totalTime + TimeSpan.FromTicks(DateTime.Now.Ticks - getLastSession().startTime.Ticks);
        }
        

        public static void saveObj()
        {
            if (obj != null)
            {
                string tempFolderPath = Path.GetTempPath();
                string filePath = Path.Combine(tempFolderPath, fileName);
                obj.save();
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    formatter.Serialize(stream, obj);
                }
            }
        }
        public static void loadObj()
        {
            string tempFolderPath = Path.GetTempPath();
            string filePath = Path.Combine(tempFolderPath, fileName);
            Console.WriteLine(filePath);
            if (!File.Exists(filePath))
            {
                createObj();
            }
            else if (obj == null) 
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    obj = (AppControle)formatter.Deserialize(stream);
                }
                obj.load();
                Console.WriteLine(obj.getNowTotalTime().ToString(@"hh\:mm\:ss"));
            }
        }

        private static void createObj()
        {
            obj = new AppControle();
        }
    }
}
