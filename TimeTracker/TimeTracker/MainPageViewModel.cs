using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTracker
{
    public class MainPageViewModel
    {
        public List<List<string>> Entries => new List<List<string>>(){new List<string>{"Hello", "goodbye"}, new List<string>{"text","more"}};
    }
}
