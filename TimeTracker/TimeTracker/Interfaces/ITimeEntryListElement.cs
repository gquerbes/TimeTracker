using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.Database;

namespace TimeTracker.Interfaces
{
    public interface ITimeEntryListElement
    {
        string Comments { get; }

        string RunTimeText { get; }

        Ticket Ticket { get; set; }
    }
}
