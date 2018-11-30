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

        public Gamemode(String name, Int16 seconds, Boolean timeIsDirty) 
        {
            _name = name;
            
        }
    }
}
