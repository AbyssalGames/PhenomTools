using System;
using System.Collections.Generic;
using UnityEngine;
using PhenomTools;

namespace BlackBoxVR.App
{
    public class UI : PersistentSingleton<UI>
    {
        [Serializable]
        private class ContextModalDict : SerializableDictionaryBase<string, ContextModal>{}
        [SerializeField]
        private ContextModalDict contextModalPrefabs = null;
        [SerializeField]
        private RectTransform genericCanvas = null;

        public static ContextModal GetNewContextModal(string prefabKey = "Default")
        {
            return Instantiate(Instance.contextModalPrefabs[prefabKey], Instance.genericCanvas);
        }
        public static ContextModal GetNewContextModal(List<ContextModal.Data> dataList, bool canDismiss = false, string prefabKey = "Default")
        {
            ContextModal contextModal = Instantiate(Instance.contextModalPrefabs[prefabKey], Instance.genericCanvas);
            contextModal.Setup(dataList, canDismiss);
            return contextModal;
        }
    }
}
