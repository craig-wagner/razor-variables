using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsertIntoTextArea.Attributes
{
    public class TokenGroupAttribute : Attribute
    {
        public string GroupName { get; set; }

        public TokenGroupAttribute( string groupName )
        {
            GroupName = groupName;
        }
    }
}