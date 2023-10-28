using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using Core;

namespace Configurations
{
    public class ConfigurationManager : MonoBehaviour
    {
        public static ConfigurationManager Instance { get; private set; }
        Configuration _configs;

        Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

        public event Action<Configuration> OnConfigurationUpdate;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);

                _configs = new Configuration();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    InitializeFirebase();
                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
        }

        public Configuration GetConfig()
        {
            return _configs;
        }

        void InitializeFirebase()
        {
            Dictionary<string, object> defaults = new Dictionary<string, object>();
            defaults.Add("MenuConfig", new MenuConfig());

            FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
                    .ContinueWithOnMainThread(task => { FetchFireBase(); });
        }

        public void FetchFireBase()
        {
            FetchDataAsync();
        }

        // FetchAsync only fetches new data if the current data is older than the provided
        // timespan. Otherwise it assumes the data is "recent enough" and does nothing.
        private Task FetchDataAsync()
        {
            Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero); // TimeSpan is zero so that changes in the console will update immediately
            return fetchTask.ContinueWith(FetchComplete);
        }

        private void FetchComplete(Task fetchTask)
        {
            if (!fetchTask.IsCompleted)
            {
                Debug.LogError("Retrieval hasn't finished.");
                return;
            }

            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            var info = remoteConfig.Info;
            if (info.LastFetchStatus != LastFetchStatus.Success)
            {
                Debug.LogErrorFormat("[0] was unsuccessful\n[1]: [2]", nameof(FetchComplete), nameof(info.LastFetchStatus), info.LastFetchStatus);
                return;
            }

            remoteConfig.ActivateAsync()
              .ContinueWithOnMainThread(
                task => {
                    _configs = Tools.ParseFromJson<Configuration>(FirebaseRemoteConfig.DefaultInstance.GetValue("GameTexts").StringValue);
                    InvokeUpdatedConfigurations();
                });
        }

        private void InvokeUpdatedConfigurations()
        {
            OnConfigurationUpdate?.Invoke(_configs);
        }
    }
}

