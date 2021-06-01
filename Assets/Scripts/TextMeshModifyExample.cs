using UnityEngine;
using UnityEngine.UI;

namespace BlueNoah
{
    public class TextMeshModifyExample : MonoBehaviour
    {
        [SerializeField]
        public Button straightButton;
        [SerializeField]
        public Button straightAsyncButton;
        [SerializeField]
        public Button spiralButton;
        [SerializeField]
        public Button spiralButton1;
        [SerializeField]
        TextMeshModify textMeshModify;

        [SerializeField]
        TextMeshMotionBase textMeshStraightLine;
        [SerializeField]
        TextMeshMotionBase textMeshSpiral;
        [SerializeField]
        TextMeshMotionBase textMeshSpiral1;
        [SerializeField]
        TextMeshMotionBase textMeshStraightLineAsync;

        [SerializeField]
        string currentText = "";

        private void Awake()
        {
            Application.targetFrameRate = 30;
            straightButton.onClick.AddListener(() =>
            {
                textMeshStraightLine.StartMotion("TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1");
            });
            straightAsyncButton.onClick.AddListener(() =>
            {
                textMeshStraightLineAsync.StartMotion("TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1");
            });
            spiralButton.onClick.AddListener(() =>
            {
                textMeshSpiral.StartMotion("TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1");
            });
            spiralButton1.onClick.AddListener(() =>
            {
                textMeshSpiral1.StartMotion("TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1TOEIC 600点：総仕上げ1");
            });
        }
    }
}

