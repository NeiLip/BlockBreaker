using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public static class Tools
    {

        // Try parsing stringed Json into T. In case parsing has failed, return default T.
        public static T ParseFromJson<T>(string jsonString)
        {
            if (!string.IsNullOrEmpty(jsonString))
            {
                try
                {
                    return JsonUtility.FromJson<T>(jsonString);
                }
                catch (System.Exception)
                {
                    throw new System.Exception("# Could not parse Json");
                }
            }

            return default;
        }

        // Try to load scene by sceneIndex
        public static void LoadScene(int sceneIndex)
        {
            try
            {
                SceneManager.LoadScene(sceneIndex);
            }
            catch (System.Exception)
            {
                throw new System.Exception("# Could not load scene: " + sceneIndex);
            }
        }
    }
}

