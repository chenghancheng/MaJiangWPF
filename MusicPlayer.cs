using System.Media;

namespace MaJiangApp
{
    public class MusicPlayer
    {
        private SoundPlayer _player;

        public MusicPlayer(string path)
        {
            _player = new SoundPlayer(path);
        }

        public void Play()
        {
            _player.PlayLooping();
        }

        public void Stop()
        {
            _player.Stop();
        }
    }
}
