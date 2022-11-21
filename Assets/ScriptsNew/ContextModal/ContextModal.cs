using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

namespace BlackBoxVR.App
{
    public class ContextModal : MonoBehaviour
    {
        [Serializable]
        public class Data
        {
            public Sprite sprite;
            public string title;
            public string body;
            public Color? color;
            //public Action callback;
            public UnityEvent callback;
            public bool spriteIsSVG;
            public bool closeOnClick = true;

            public Data() { }
            public Data(UnityAction action)
            {
                callback = new UnityEvent();
                callback.AddListener(action);
            }
        }

        public event Action onClose;

        [SerializeField]
        private TextMeshProUGUI titleText = null;
        [SerializeField]
        private TextMeshProUGUI bodyText = null;
        [SerializeField]
        private Button dismissButton = null;
        [SerializeField]
        private Button closeButton = null;
        [SerializeField]
        private ContextModalSection contextModalSectionPrefab = null;
        [SerializeField]
        private Transform sectionParent = null;

        private List<ContextModalSection> sections = new List<ContextModalSection>();

        public void Setup(string title, string body, List<Data> dataList, bool canDismiss = false)
        {
            if (!string.IsNullOrEmpty(title))
            {
                titleText.gameObject.SetActive(true);
                titleText.SetText(title);
            }
            if (!string.IsNullOrEmpty(body))
            {
                bodyText.gameObject.SetActive(true);
                bodyText.SetText(body);
            }

            Setup(dataList);
        }

        public void Setup(List<Data> dataList, bool canDismiss = false)
        {
            dismissButton.enabled = canDismiss;
            closeButton.gameObject.SetActive(canDismiss);

            for (int i = 0; i < sections.Count; i++)
                Destroy(sections[i].gameObject);

            sections.Clear();

            foreach (Data data in dataList)
            {
                ContextModalSection newSection = Instantiate(contextModalSectionPrefab, sectionParent);
                newSection.Initialize(this, data);
                sections.Add(newSection);
            }
        }

        public void Close()
        {
            onClose?.Invoke();
            Destroy(gameObject);
        }
    }
}
