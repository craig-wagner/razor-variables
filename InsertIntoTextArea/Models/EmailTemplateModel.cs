using System.ComponentModel;
using InsertIntoTextArea.Attributes;

namespace InsertIntoTextArea.Models
{
    public class EmailTemplateModel
    {
        [HideInTemplateEditor]
        public int CaseId { get; set; }

        [Description( "First Name" )]
        [TokenGroup( "Reporter" )]
        public string FirstName { get; set; }

        [Description( "Last Name" )]
        [TokenGroup( "Reporter" )]
        public string LastName { get; set; }

        [Description( "Case URL" )]
        [TokenGroup( "Case" )]
        public string CaseUrl { get; set; }

        [TokenGroup( "Case" )]
        public string Tier { get; set; }

        public string SomethingElse { get; set; }
    }
}
