using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLTablo_WPF
{
    public class Gamemode
    {
        private string _name;
        private TimeSpan _totalTime;
        private bool _timeIsDirty;

        List<Int16> scores;

        private int maxDiff;
        private int maxClash;

        public String Name { get { return _name; } }
        public TimeSpan TotalTime { get { return _totalTime; } }
        public Boolean TimeIsDirty { get { return _timeIsDirty; } }

        public Gamemode(String name, Int16 seconds, Boolean timeIsDirty) 
        {
            _name = name;
            _totalTime = new TimeSpan(0, 0, seconds);
            _timeIsDirty = timeIsDirty;
        }
    }
}
