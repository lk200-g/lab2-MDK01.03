using System;
using System.Collections.Generic;

namespace lab2
{
    class LabSortedSet
    {
        public SortedSet<Lamp> Lamps = new SortedSet<Lamp>();
        public DateTime InitDate { get; }
        public int Length { get { return Lamps.Count; } }
        public string Type { get { return "SortedSet<Lamp>"; } }
        public LabSortedSet()
        {
            InitDate = DateTime.Now;
        }
    }
}
