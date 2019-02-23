using System;
using System.Collections.Generic;
using System.Text;
using TimeTracker.Database;
using TimeTracker.Models.Replicon.RepliconReply;

namespace TimeTracker.Interfaces
{
    public interface ITimeEntryListElement
    {
        string Comments { get; }

        string RunTimeText { get; }

        RepliconTask Ticket { get; set; }
    }
}
