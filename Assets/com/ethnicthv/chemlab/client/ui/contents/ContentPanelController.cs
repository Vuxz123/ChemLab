using System;
using com.ethnicthv.chemlab.client.api.ui.contents;
using com.ethnicthv.chemlab.engine.api.mixture;
using TMPro;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.ui.contents
{
    public class ContentPanelController : MonoBehaviour, IContentPanelController
    {
        [SerializeField] private ContentListController contentListController;
        [SerializeField] private TextMeshProUGUI temperatureText;
        [SerializeField] private TextMeshProUGUI volumnText;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void ClosePanel()
        {
            gameObject.SetActive(false);
        }

        public void OpenPanel()
        {
            gameObject.SetActive(true);
            gameObject.transform.SetAsLastSibling();
        }

        public void SetupMixtureToDisplay(IMixture mixture, float volumn)
        {
            contentListController.Setup(mixture, volumn);
            SetTemperatureText(mixture);
            SetVolumnText(volumn);
        }

        private void SetTemperatureText(IMixture mixture)
        {
            if (mixture == null)
            {
                temperatureText.text = "Nhiệt độ: không rõ";
                return;
            }
            var temperature = mixture.GetTemperature();
            var celsiusDegree = temperature - 273.15f;
            var roundedCelsiusDegree = Math.Round(celsiusDegree, 2);
            //Note: round to 2 decimal places
            temperatureText.text = $"Nhiệt độ: {roundedCelsiusDegree}°C"; 
        }

        private void SetVolumnText(float volumn)
        {
            if (volumn <= 0)
            {
                volumnText.text = "Dung tích: rỗng!";
                return;
            }
            volumnText.text = volumn < 1 ? 
                $"Dung tích: {Mathf.Round(volumn * 1000)} mL" : 
                $"Dung tích: {Mathf.Round(volumn)} L";
        }
    }
}