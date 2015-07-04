using System.Collections.Generic;

namespace InsertIntoTextArea.Models
{
    public class TemplateBuilderViewModel
    {
        public Dictionary<string, List<KeyValuePair<string, string>>> AvailableFields { get; private set; }

        public TemplateBuilderViewModel()
        {
            AvailableFields = new Dictionary<string, List<KeyValuePair<string, string>>>();
        }
    }
}