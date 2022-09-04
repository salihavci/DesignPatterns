using System.Drawing;
using System.IO;

namespace WebApp.Adapter.Services
{
    public class AdvanceImageProcessAdapter:IImageProcess
    {
        private readonly IAdvanceImageProcess _advanceImageProcess;

        public AdvanceImageProcessAdapter(IAdvanceImageProcess advanceImageProcess)
        {
            _advanceImageProcess = advanceImageProcess;
        }

        public void AddWatermark(string text, string fileName, Stream imageStream)
        {
            _advanceImageProcess.AddWatermarkImage(imageStream, text, $"wwwroot/watermarks/{fileName}", Color.FromArgb(128, 0, 0, 0), Color.FromArgb(128, 255, 0, 0));
        }
    }
}
