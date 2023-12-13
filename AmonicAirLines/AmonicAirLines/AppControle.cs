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
using FluentAssertions.Common;

namespace AmonicAirLines
{
    public enum ReasonCrash
    {
        Zero,
        System,
        Software,
    }
    [Serializable] 
    public class Session
    {
        public TimeSpan totalTime;
        public DateTime startTime;
        public DateTime endTime;
        public bool crash;
        public ReasonCrash reason;
        public string crashDescription;
        public Session()
        {
            totalTime = TimeSpan.Zero;
            startTime = DateTime.Now;
            endTime = DateTime.MinValue;
            crash = true;
            reason = ReasonCrash.Zero;
            crashDescription = "";
        }
        public void endSession()
        {
            crash = false;
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
        public List<Session> sessions = new List<Session>();

        public int getCountCrashes() => AppControle.obj.sessions
            .Take(AppControle.obj.sessions.Count - 1)
            .Count(session => session.crash);
        public bool lastSessionIsCrash()
        {
            if (sessions.Count > 1)
            {
                return sessions[sessions.Count - 2].crash;
            }
            return false;
        }
        public void setReasonCrash(bool c, string descrition)
        {
            if (sessions.Count > 1)
            {
                if (c)
                    sessions[sessions.Count - 2].reason = ReasonCrash.System;
                else
                    sessions[sessions.Count - 2].reason = ReasonCrash.Software;
                sessions[sessions.Count - 2].crashDescription = descrition;
                OnlySaveFile();

            }
        }
        public TimeSpan getNowTotalTime()
        {
            Console.Write($"start = {getLastSession().startTime}\t");
            Console.Write($"now = {DateTime.Now}\t");
            Console.Write($"total = {getLastSession().endTime - getLastSession().startTime}\ttotalTime{totalTime + TimeSpan.FromTicks(DateTime.Now.Ticks - getLastSession().startTime.Ticks)}\n");
            return totalTime + TimeSpan.FromTicks(DateTime.Now.Ticks - getLastSession().startTime.Ticks);
        }
        
        public void printSession()
        {
            Console.WriteLine($"session count = {sessions.Count}\t");
            for (int i = 0; i < sessions.Count; ++i)
            {
                Console.WriteLine($"\tsessionID: {i}" +
                    $"\ttotalTime: {sessions[i].totalTime}" +
                    $"\tstartTime: {sessions[i].startTime}" +
                    $"\tendTime: {sessions[i].endTime}" +
                    $"\tcrash: {sessions[i].crash}" +
                    $"\tReason: {sessions[i].reason}" +
                    $"\tDiscription: {sessions[i].crashDescription}");
            }
        }
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

        private static void OnlySaveFile()
        {

            string tempFolderPath = Path.GetTempPath();
            string filePath = Path.Combine(tempFolderPath, fileName);
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, obj);
            }
        }
        public static void saveObj()
        {
            if (obj != null)
            {
                obj.save();
                OnlySaveFile();

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
                OnlySaveFile();
            }
        }

        private static void createObj()
        {
            obj = new AppControle();
        }
    }
}
