using System;
using Robust.Client.Graphics;

namespace OpenDreamClient.Audio
{
    public class DreamSoundChannel : IDisposable {
        public IClydeAudioSource Source { get; }

        public DreamSoundChannel(IClydeAudioSource source) {
            Source = source;
        }

        public void Stop() {
            Source?.StopPlaying();
        }

        public void Dispose() {
            Stop();
            Source?.Dispose();
        }
    }
}
