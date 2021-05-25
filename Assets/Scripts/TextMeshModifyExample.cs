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
        TextMeshBase textMeshSpiral;

        private void Awake()
        {
            circleButton.onClick.AddListener(() =>
            {
                textMeshModify.StartCircle();
            });
            circleButton1.onClick.AddListener(() =>
            {
                textMeshModify.StartCircle1();
            });
            spiralButton.onClick.AddListener(() =>
            {
                textMeshSpiral.StartMotion();
            });
        }
    }
}

