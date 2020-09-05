using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Planetarity.UI
{
    public class GameResultSequenceView : MonoBehaviour
    {
        [SerializeField] private Text _resultText;
        [SerializeField] private float _showSeconds = 3;

        public async Task Show(string resultText)
        {
            _resultText.text = resultText;
            gameObject.SetActive(true);
            await Task.Delay(TimeSpan.FromSeconds(_showSeconds));
            gameObject.SetActive(false);
        }
    }
}