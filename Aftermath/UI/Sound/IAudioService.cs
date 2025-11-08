using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Aftermath.UI.Sound
{
    public interface IAudioService
    {
        void PlayHealSound();
    }



    public class AudioService : IAudioService
    {
        private IWavePlayer outputDevice;
        private AudioFileReader audioFile;

        public void PlayHealSound()
        {
            Play("Resources/Sounds/HealSound.wav", 0.3f);
        }

        private void Play(string path, float volume)
        {
            Stop();

            audioFile = new AudioFileReader(path) { Volume = volume };
            outputDevice = new WaveOutEvent();
            outputDevice.Init(audioFile);
            outputDevice.Play();
        }

        public void Stop()
        {
            outputDevice?.Stop();
            audioFile?.Dispose();
            outputDevice?.Dispose();
            outputDevice = null;
            audioFile = null;
        }
    }
}
