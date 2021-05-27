using UnityEngine;
using UnityEngine.UI;

namespace BlueNoah
{
    public class TextMeshModifyExample : MonoBehaviour
    {
        [SerializeField]
        public Button circleButton;
        [SerializeField]
        public Button circleButton1;
        [SerializeField]
        public Button spiralButton;
        [SerializeField]
        TextMeshModify textMeshModify;

        [SerializeField]
        TextMeshMotionBase textMeshSpira;
        [SerializeField]
        TextMeshMotionBase textMeshSpiral;

        [SerializeField]
        string currentText = "";

        private void Awake()
        {
            circleButton.onClick.AddListener(() =>
            {
                textMeshModify.StartCircle();
            });
            circleButton1.onClick.AddListener(() =>
            {
                textMeshSpira.StartMotion("TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1");
            });
            spiralButton.onClick.AddListener(() =>
            {
                textMeshSpiral.StartMotion("TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1");
            });
        }
    }
}

