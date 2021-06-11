using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlueNoah
{
    public class TextMeshModifyExample : MonoBehaviour
    {
        [SerializeField]
        public Button straightButton;
        [SerializeField]
        public Button straightButton1;
        [SerializeField]
        public Button spiralButton;
        [SerializeField]
        public Button spiralButton1;
        [SerializeField]
        TextMeshModify textMeshModify;
        [SerializeField]
        TMP_InputField inputField;
        [SerializeField]
        TextMeshProUGUI inputFieldText;
        const int CharacterLimit = 50;

        [SerializeField]
        TextMeshMotionBase textMeshStraightLine;
        [SerializeField]
        TextMeshMotionBase textMeshSpiral;
        [SerializeField]
        TextMeshMotionBase textMeshSpiral1;
        [SerializeField]
        TextMeshMotionBase textMeshStraight1;

        [SerializeField]
        string currentText = "TOEIC 600点TOEIC 600点TOEIC 600点TOEIC 600点TOEIC 600点TOEIC 600点";

        private void Awake()
        {
            Application.targetFrameRate = 30;
            straightButton.onClick.AddListener(() =>
            {
                textMeshStraightLine.StartMotion(currentText, true,()=> { });
            });
            straightButton1.onClick.AddListener(() =>
            {
                textMeshStraight1.StartMotion(currentText, true, () => { });
            });
            spiralButton.onClick.AddListener(() =>
            {
                textMeshSpiral.StartMotion(currentText, true, () => { });
            });
            spiralButton1.onClick.AddListener(() =>
            {
                textMeshSpiral1.StartMotion(currentText, true, () => { });
            });
        }

        private void Start()
        {
#if UNITY_EDITOR
            textMeshStraightLine.StartMotion(currentText, true, () => { });
            textMeshStraight1.StartMotion(currentText, true, () => { });
            textMeshSpiral.StartMotion(currentText, true, () => { });
            textMeshSpiral1.StartMotion(currentText, true, () => { });
#endif
        }
        private void LateUpdate()
        {
            if(inputFieldText!= null && inputFieldText.gameObject.activeInHierarchy)
                UpdateTextCharactor();
        }

        TMP_Text tmpText;
        TMP_TextInfo textInfo;
        /// <summary>
        /// 255文字後の文字が赤いのなる
        /// </summary>
        void UpdateTextCharactor()
        {
            tmpText = inputFieldText.GetComponent<TMP_Text>();
            Color32 c0 = tmpText.color;
            textInfo = tmpText.textInfo;
            var red0 = new Color(1, 150f/255, 0 / 255, 1);
            var red1 = new Color(1, 80f/255, 0 / 255, 1);
            var red2 = new Color(1, 0, 0, 1);
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                var newVertexColors = textInfo.meshInfo[materialIndex].colors32;
                if (textInfo.characterInfo[i].isVisible)
                {
                    if (i >= CharacterLimit)
                    {
                        newVertexColors[vertexIndex + 0] = red1;
                        newVertexColors[vertexIndex + 1] = red0;
                        newVertexColors[vertexIndex + 2] = red1;
                        newVertexColors[vertexIndex + 3] = red2;
                    }
                    else
                    {
                        newVertexColors[vertexIndex + 0] = c0;
                        newVertexColors[vertexIndex + 1] = c0;
                        newVertexColors[vertexIndex + 2] = c0;
                        newVertexColors[vertexIndex + 3] = c0;
                    }
                }
            }
            tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }
    }
}

