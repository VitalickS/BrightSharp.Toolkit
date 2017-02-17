using CefSharp;
using System;
using System.IO;
using System.Net;

namespace BrightSharp.Toolkit.Extra
{

    public class FolderSchemeHandlerFactory : ISchemeHandlerFactory
    {
        private string rootFolder;
        private string defaultPage;
        private string schemeName;
        private string hostName;

        public FolderSchemeHandlerFactory(string rootFolder, string schemeName = null, string hostName = null, string defaultPage = "index.html")
        {
            this.rootFolder = Path.GetFullPath(rootFolder);
            this.defaultPage = defaultPage;
            this.schemeName = schemeName;
            this.hostName = hostName;

            if (!Directory.Exists(this.rootFolder))
            {
                throw new DirectoryNotFoundException(this.rootFolder);
            }
        }

        IResourceHandler ISchemeHandlerFactory.Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            if (this.schemeName != null && !schemeName.Equals(this.schemeName, StringComparison.OrdinalIgnoreCase))
            {
                var invalidSchemeName = ResourceHandler.FromString(string.Format("SchemeName {0} does not match the expected SchemeName of {1}.", schemeName, this.schemeName));
                invalidSchemeName.StatusCode = (int)HttpStatusCode.NotFound;

                return invalidSchemeName;
            }
            
            var uri = new Uri(request.Url);

            if (hostName != null && !uri.Host.Equals(hostName, StringComparison.OrdinalIgnoreCase))
            {
                var invalidHostName = ResourceHandler.FromString(string.Format("HostName {0} does not match the expected HostName of {1}.", uri.Host, hostName));
                invalidHostName.StatusCode = (int)HttpStatusCode.NotFound;

                return invalidHostName;
            }

            //Get the absolute path and remove the leading slash
            var asbolutePath = uri.Host + uri.AbsolutePath;

            if (string.IsNullOrEmpty(asbolutePath))
            {
                asbolutePath = defaultPage;
            }

            var filePath = Path.GetFullPath(Path.Combine(rootFolder, asbolutePath));

            //Check the file requested is within the specified path and that the file exists
            if (filePath.StartsWith(rootFolder, StringComparison.OrdinalIgnoreCase) && File.Exists(filePath))
            {
                var fileExtension = Path.GetExtension(filePath);
                var mimeType = ResourceHandler.GetMimeType(fileExtension);
                return ResourceHandler.FromFilePath(filePath, mimeType);
            }

            var fileNotFoundResourceHandler = ResourceHandler.FromString("File Not Found - " + filePath);
            fileNotFoundResourceHandler.StatusCode = (int)HttpStatusCode.NotFound;

            return fileNotFoundResourceHandler;
        }
    }
}