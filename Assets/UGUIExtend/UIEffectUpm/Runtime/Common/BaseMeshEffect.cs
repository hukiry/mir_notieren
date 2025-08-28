using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HukiryEffects
{
    /// <summary>
    /// Base class for effects that modify the generated Mesh.
    /// It works well not only for standard Graphic components (Image, RawImage, Text, etc.) but also for TextMeshPro and TextMeshProUGUI.
    /// </summary>
    [RequireComponent(typeof(Graphic))]
    [RequireComponent(typeof(RectTransform))]
    [ExecuteInEditMode]
    public abstract class BaseMeshEffect : UIBehaviour, IMeshModifier
    {
        RectTransform _rectTransform;
        Graphic _graphic;
        GraphicConnector _connector;

        /// <summary>
        /// The Graphic attached to this GameObject.
        /// </summary>
        protected GraphicConnector connector
        {
            get { return _connector ?? (_connector = GraphicConnector.FindConnector(graphic)); }
        }

        /// <summary>
        /// The Graphic attached to this GameObject.
        /// </summary>
        public Graphic graphic
        {
            get { return _graphic ? _graphic : _graphic = GetComponent<Graphic>(); }
        }

        /// <summary>
        /// The RectTransform attached to this GameObject.
        /// </summary>
        protected RectTransform rectTransform
        {
            get { return _rectTransform ? _rectTransform : _rectTransform = GetComponent<RectTransform>(); }
        }

        internal readonly List<UISyncEffect> syncEffects = new List<UISyncEffect>(0);
        public virtual void ModifyMesh(Mesh mesh)
        {
        }

        /// <summary>
        /// Call used to modify mesh.
        /// </summary>
        /// <param name="vh">VertexHelper.</param>
        public virtual void ModifyMesh(VertexHelper vh)
        {
            ModifyMesh(vh, graphic);
        }

        public virtual void ModifyMesh(VertexHelper vh, Graphic graphic)
        {
        }

        /// <summary>
        /// Mark the vertices as dirty.
        /// </summary>
        protected virtual void SetVerticesDirty()
        {
            connector.SetVerticesDirty(graphic);

            foreach (var effect in syncEffects)
            {
                effect.SetVerticesDirty();
            }
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        protected override void OnEnable()
        {
            connector.OnEnable(graphic);
            SetVerticesDirty();
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled () or inactive.
        /// </summary>
        protected override void OnDisable()
        {
            connector.OnDisable(graphic);
            SetVerticesDirty();
        }

        /// <summary>
        /// Mark the effect parameters as dirty.
        /// </summary>
        protected virtual void SetEffectParamsDirty()
        {
            if (!isActiveAndEnabled) return;
            SetVerticesDirty();
        }

        /// <summary>
        /// Callback for when properties have been changed by animation.
        /// </summary>
        protected override void OnDidApplyAnimationProperties()
        {
            if (!isActiveAndEnabled) return;
            SetEffectParamsDirty();
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            if (!isActiveAndEnabled) return;
            SetVerticesDirty();
        }

        /// <summary>
        /// This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only).
        /// </summary>
        protected override void OnValidate()
        {
            if (!isActiveAndEnabled) return;
            SetEffectParamsDirty();
        }
#endif
    }
}
