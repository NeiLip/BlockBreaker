using UnityEngine;
using UnityEngine.UI;
using Core;
using Configurations;

namespace Menu
{
	public class MenuUI : MonoBehaviour
	{
        [SerializeField] Text _title_Text;
        [SerializeField] Text _subTitle_Text;

		private void Start ()
        {
			UpdateTexts(ConfigurationManager.Instance.GetConfig());

			ConfigurationManager.Instance.OnConfigurationUpdate += UpdateTexts;
		}

        private void OnDisable()
        {
			ConfigurationManager.Instance.OnConfigurationUpdate -= UpdateTexts;
		}

        public void UpdateTexts(Configuration config)
        {
			_title_Text.text = config.menuConfig.menuTexts.title;
			_subTitle_Text.text = config.menuConfig.menuTexts.subTitle;
		}

        //Called when the play button is pressed
        public void PlayButton()
		{
			Tools.LoadScene(Constants.GAME_SCENE);
		}

		//Called when the quit button is pressed
		public void QuitButton()
		{
			Application.Quit();
		}
	}
}
