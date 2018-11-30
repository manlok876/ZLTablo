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

        private int _maxScoreGap;
        private int _maxDoubleHits;

        public String Name { get { return _name; } }
        public TimeSpan TotalTime { get { return _totalTime; } }
        public Boolean TimeIsDirty { get { return _timeIsDirty; } }

        public Boolean CountScoreGap { get { return _maxScoreGap >= 0; } }
        public Boolean CountDoubleHits { get { return _maxDoubleHits >= 0; } }

        public Int32 MaxDoubleHits { get { return _maxDoubleHits; } }
        public Int32 MaxScoreGap { get { return _maxScoreGap; } }

        public Gamemode(String name, Int32 seconds, Boolean timeIsDirty, Int32 maxScoreGap = -1, Int32 maxDoubleHits = -1) 
        {
            _name = name;
            _totalTime = new TimeSpan(0, 0, seconds);
            _timeIsDirty = timeIsDirty;
            _maxScoreGap = maxScoreGap;
            _maxDoubleHits = maxDoubleHits;
        }
    }
}
