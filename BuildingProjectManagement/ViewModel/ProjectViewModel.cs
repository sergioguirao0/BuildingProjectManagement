using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingProjectManagement.ViewModel
{
    public class ProjectViewModel : ViewModelBase
    {
        private string? _projectChecksMessage;
        public string? ProjectChecksMessage
        {
            get => _projectChecksMessage;
            set
            {
                if (_projectChecksMessage != value)
                {
                    _projectChecksMessage = value;
                    OnPropertyChanged(nameof(ProjectChecksMessage));
                }
            }
        }
    }
}
