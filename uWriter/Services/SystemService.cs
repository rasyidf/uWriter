using System.Diagnostics;

using uWriter.Contracts.Services;

namespace uWriter.Services
{
    public class SystemService : ISystemService
    {
        public SystemService()
        {
        }

        public void OpenInWebBrowser(string url)
        {

            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
