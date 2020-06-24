using System;
using System.Collections.Generic;
using System.Text;

namespace MonogoPoc
{
    public class LocalUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id} - {nameof(Name)}: {Name} - {nameof(Date)}: {Date} - {nameof(DateTimeOffset)}: {DateTimeOffset}";
        }
    }
}
