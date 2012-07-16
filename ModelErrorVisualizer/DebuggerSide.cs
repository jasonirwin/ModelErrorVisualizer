using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(TestVisualizer.DebuggerSide),
Target = typeof(ModelStateDictionary),
Description = "Model Error Visualizer")]
namespace TestVisualizer
{
    public class DebuggerSide : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var obj = objectProvider.GetObject() as ModelStateDictionary;
            var errors = new List<ModelStateError>();

            foreach (var propertyInfo in obj.Where(x=>x.Value.Errors.Any()))
            {
                foreach (var error in propertyInfo.Value.Errors)
                {

                    errors.Add(new ModelStateError(propertyInfo.Key, error.ErrorMessage));
                   
                }
            }
            
            
            var d = new CodeViewer(errors);
            
            
            windowService.ShowDialog(d);
        }

        private static string BuildInitializationString(IEnumerable<PropertyInfo> properties, Object obj)
        {
            var sb = new StringBuilder();
            foreach (var property in properties)
            {
                // Canread - used because we need to be able to read it here to get its value
                // Canwrite - do not want to script out properties with read-only values
                if (property.PropertyType.IsPublic && property.CanWrite && property.CanRead)
                {
                    if (!property.PropertyType.IsValueType)
                    {
                        if ((property.PropertyType == typeof(string)))
                        {
                            sb.Append(CreateStringInitializationString(property, obj));
                        }
                        else
                        {
                            sb.Append(string.Format("{0}=new {1}() {{ ", property.Name, property.PropertyType));
                            var str = BuildInitializationString(property.PropertyType.GetProperties(),property.GetValue(obj, null));
                            str = str.Remove(str.LastIndexOf(","), 1);
                            sb.Append(str);
                            sb.Append("},");
                        }
                    }
                    else
                    {
                            sb.Append(CreateValueTypeInitializationString(property, obj));
                    }
                }
            }
            return sb.ToString();
        }

        private static string CreateStringInitializationString(PropertyInfo property, object obj)
        {
            return string.Format("{0}=\"{1}\", ", property.Name, property.GetValue(obj, null));
        }

        private static string CreateValueTypeInitializationString(PropertyInfo property, object obj)
        {
            return string.Format("{0}={1}, ", property.Name, property.GetValue(obj, null));
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            var visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(DebuggerSide));
            visualizerHost.ShowVisualizer();
        }
    }

    public class ModelStateError

    {
        public ModelStateError(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }

        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }

    }
}
