using System;

namespace Sources.Services.Scene
{
    public interface ISceneService
    {
        public void Load(int sceneIndex, bool useCurtain, Action onLoad = null);
        public void LoadImmediately(int sceneIndex, Action onLoad = null);
        public void LoadAdditive(int sceneIndex, Action onLoad = null);
        public void Unload(int sceneIndex, Action onUnload = null);
    }
}
