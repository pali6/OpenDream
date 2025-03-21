using System.Collections.Generic;
using OpenDreamClient.Interface.Controls;
using OpenDreamShared.Interface;
using Robust.Shared.Timing;

namespace OpenDreamClient.Interface
{
    /// <summary>
    /// Used in unit testing to run a headless client.
    /// </summary>
    public class DummyDreamInterfaceManager : IDreamInterfaceManager
    {
        public string[] AvailableVerbs { get; }
        public Dictionary<string, ControlWindow> Windows { get; }
        public InterfaceDescriptor InterfaceDescriptor { get; }

        public void Initialize()
        {

        }

        public void FrameUpdate(FrameEventArgs frameEventArgs)
        {

        }
        public InterfaceElement FindElementWithName(string name)
        {
            return null;
        }
        public void SaveScreenshot(bool openDialog)
        {

        }
        public void LoadInterfaceFromSource(string source)
        {

        }
    }
}
