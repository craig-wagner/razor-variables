using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using InsertIntoTextArea.Attributes;
using InsertIntoTextArea.Models;
using Xipton.Razor;
using Xipton.Razor.Core;

namespace InsertIntoTextArea.Controllers
{
    public class HomeController : Controller
    {
        private static Random random = new Random( (int)DateTime.Now.Ticks );

        public ActionResult Index()
        {
            var viewModel = new TemplateBuilderViewModel();

            var properties = typeof( EmailTemplateModel ).GetProperties();

            foreach( var property in properties )
            {
                var hideAttr = property.GetCustomAttribute( typeof( HideInTemplateEditorAttribute ) );

                if( hideAttr == null )
                {
                    var descriptionAttr = property.GetCustomAttribute( typeof( DescriptionAttribute ) ) as DescriptionAttribute;
                    var groupAttr = property.GetCustomAttribute( typeof( TokenGroupAttribute ) ) as TokenGroupAttribute;

                    var labelText = descriptionAttr != null ? descriptionAttr.Description : property.Name;
                    var groupName = groupAttr != null ? groupAttr.GroupName : "Misc";

                    if( !viewModel.AvailableFields.ContainsKey( groupName ) )
                    {
                        viewModel.AvailableFields.Add( groupName, new List<KeyValuePair<string, string>>() );
                    }

                    viewModel.AvailableFields[groupName].Add( new KeyValuePair<string, string>( labelText, property.Name ) );
                }
            }

            return View( viewModel );
        }

        public ActionResult Preview( string body )
        {
            RazorMachine machine = new RazorMachine();
            var preamble = GeneratePreamble();

            var model = new EmailTemplateModel();

            var properties = model.GetType().GetProperties();

            foreach( var property in properties )
            {
                if( property.PropertyType == typeof( System.String ) )
                {
                    property.SetValue( model, RandomString( 8 ) );
                }
                else if( property.PropertyType == typeof( System.Int32 ) )
                {
                    property.SetValue( model, 42 );
                }
            }

            var content = machine.ExecuteContent( preamble + body, model ).Result;

            return Content( "<pre>" + content + "</pre>", "text/html" );
        }

        public ActionResult Validate( string body )
        {
            ActionResult result = null;

            RazorMachine machine = new RazorMachine();

            try
            {
                var preamble = GeneratePreamble();
                machine.ExecuteContent( preamble + body, new EmailTemplateModel() );

                result = Content( "Template is good! You get a cookie!", "text/plain" );
            }
            catch( TemplateException ex )
            {
                const string code = "CS0103";

                var lines = ex.Message.Split( new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries );
                var errors = (from l in lines
                              where l.Contains( code )
                              select l.Substring( l.IndexOf( code ) + code.Length + 2 ))
                             .ToList();

                StringBuilder errorMsg = new StringBuilder( "You screwed it up you tool!<ul>" );

                foreach( var error in errors )
                {
                    errorMsg.Append( "<li>" );
                    errorMsg.Append( error );
                }

                errorMsg.Append( "</ul>" );

                result = Content( errorMsg.ToString(), "text/plain" );
            }

            return result;
        }

        private string GeneratePreamble()
        {
            StringBuilder variableBlock = new StringBuilder( "@{\r\nLayout = null;\r\n" );

            var properties = typeof( EmailTemplateModel ).GetProperties();

            foreach( var property in properties )
            {
                variableBlock.AppendFormat( "var {0} = Model.{0};", property.Name );
                variableBlock.AppendLine();
            }

            variableBlock.AppendLine( "}" );

            return variableBlock.ToString();
        }

        private string RandomString( int size )
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for( int i = 0; i < size; i++ )
            {
                ch = Convert.ToChar( Convert.ToInt32( Math.Floor( 26 * random.NextDouble() + 65 ) ) );
                builder.Append( ch );
            }

            return builder.ToString();
        }
    }
}