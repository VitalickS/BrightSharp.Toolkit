using AppleJobs.Data;
using BrightSharp.Mvvm;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetFrames.AppleJobs.Editor
{
    public class ViewModelLocator
    {
        AppleJobsContext context;
        public ViewModelLocator()
        {
            if (ObservableObject.IsInDesignTime)
            {
                context = new AppleJobsContext("server=138.201.230.158;user id=admin_applejobs;pwd=dPARP0etzx;database=admin_applejobs_dev;CharSet=utf8;");
            }
            else
            {
                context = new AppleJobsContext();
            }
            Editor = new EditorViewModel(context);
        }

        public EditorViewModel Editor { get; private set; }
        
    }
}
