using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class Configuration
    {
        public MenuConfig menuConfig;

        public Configuration()
        {
            menuConfig = new MenuConfig();
        }
    }

    [System.Serializable]
    public class MenuConfig
    {
        public MenuTexts menuTexts;

        public MenuConfig()
        {
            menuTexts = new MenuTexts();
        }
    }

    [System.Serializable]
    public class MenuTexts
    {
        public string title;
        public string subTitle;

        public MenuTexts()
        {
            title = "BLOCK BREAKER TEMPLATE";
            subTitle = "BREAK ALL THE BLOCKS TO WIN THE GAME";
        }

        public void Print()
        {
            Debug.LogFormat("# Menu texts. Title: {0}. SubTitle: {1}.", title, subTitle);
        }
    }
}
