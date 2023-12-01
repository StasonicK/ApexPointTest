using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner) =>
            _coroutineRunner = coroutineRunner;

        public void Load(SceneId nextSceneId, Action<SceneId> onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(nextSceneId, onLoaded));

        public void Load(SceneId nextSceneId, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(nextSceneId, onLoaded));

        private IEnumerator LoadScene(SceneId nextSceneId, Action<SceneId> onLoaded = null)
        {
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextSceneId.ToString());

            while (!waitNextScene.isDone)
                yield return null;

            onLoaded?.Invoke(nextSceneId);
        }

        private IEnumerator LoadScene(SceneId nextSceneId, Action onLoaded = null)
        {
            if (SceneManager.GetActiveScene().name == nextSceneId.ToString())
            {
                onLoaded?.Invoke();
                yield break;
            }

            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextSceneId.ToString());

            while (!waitNextScene.isDone)
                yield return null;

            onLoaded?.Invoke();
        }

        // public void Load(Scene nextScene)
        // {
        //     _coroutineRunner.StartCoroutine(LoadScene(nextScene, nu));
        // }
    }
}