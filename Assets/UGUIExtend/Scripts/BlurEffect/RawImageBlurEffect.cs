using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hukiry
{
    [RequireComponent(typeof(RawImage))]
    [ExecuteInEditMode]
    public class RawImageBlurEffect : MonoBehaviour
    {
        [SerializeField]
        public TextureBlurType textureBlurType;
        private RawImage rawImage;
        [Header("采样模糊")]
        [SerializeField]
        private Shader textureBlur;
        [Range(0, 5)]
        [SerializeField]
        private int DownSample = 1;
        [Range(0.1F, 5)]
        [SerializeField]
        private float BlurRadius = 1;
        [Range(1, 10)]
        [SerializeField]
        private int BlurIterations = 8;

        
        [Header("FastBlur")]
        [SerializeField]
        private Shader textureFastBlur;
        [Range(0f, 2f)] public int downsample_fastBlur = 1;
        [Range(0f, 10f)] public float blurSize_fastBlur = 3;
        [Range(1f, 4f)] public int blurIterations_fastBlur = 2;
        private Material blurMaterial;
        [HideInInspector]
        public Texture rawTexture;

        void Awake()
        {
            InitBlur();
        }

        private void InitBlur()
        {
            if (rawImage == null || rawTexture == null)
            {
                rawImage = GetComponent<RawImage>();
                rawTexture = rawImage.texture;
            }

            if (blurMaterial == null)
            {
                this.UpdateMaterial();
                rawImage.material = null;
            }
        }

        private void UpdateMaterial()
        {
            switch (textureBlurType)
            {
                case TextureBlurType.FastBlur:
                    if (textureFastBlur == null)
                    {
                        textureFastBlur = Shader.Find("Hidden/FastBlur");
                    }
                    blurMaterial = new Material(textureFastBlur);
                    break;
                case TextureBlurType.SampleBlur:
                    if (textureBlur == null)
                    {
                        textureBlur = Shader.Find("Custom/UITextureBlur");
                    }
                    blurMaterial = new Material(textureBlur);
                    break;
            }
        }

        void OnEnable()
        {
            this.InitBlur();
            switch (textureBlurType)
            {
                case TextureBlurType.FastBlur:
                    this.RenderImageFastBlur();
                    break;
                case TextureBlurType.SampleBlur:
                    this.ApplyBlur();
                    break;
            }
        }

        //模糊半径 = 1 ，模糊采样=1 ，模糊次数 = 8
        public void ApplyBlur(Texture rawTextureParas = null)
        {
            if (rawTextureParas)
            {
                rawTexture = rawTextureParas;
            }

            if (rawTexture == null)
            {
                return;
            }

            //先把原图赋值给RenderTexture
            RenderTexture source = new RenderTexture(rawTexture.width, rawTexture.height, 0);
            Graphics.Blit(rawTexture, source, blurMaterial);
            //DestroyImmediate(rawTexture, true);
            //申请RenderTexture，RT的分辨率按照downSample降低
            RenderTexture renderTexture1 = RenderTexture.GetTemporary(source.width >> DownSample, source.height >> DownSample, 0, source.format);
            RenderTexture renderTexture2 = RenderTexture.GetTemporary(source.width >> DownSample, source.height >> DownSample, 0, source.format);
            RenderTexture targetTexture = source;
            Graphics.Blit(source, renderTexture1);//直接将原图拷贝到降分辨率的RT上
            for (int i = 0; i < BlurIterations; i++)//进行迭代，一次迭代进行了两次模糊操作，使用两张RT交叉处理
            {
                //用降过分辨率的RT进行模糊处理
                blurMaterial.SetFloat("_BlurRadius", BlurRadius);
                Graphics.Blit(renderTexture1, renderTexture2, blurMaterial);
                Graphics.Blit(renderTexture2, renderTexture1, blurMaterial);
            }

            Graphics.Blit(renderTexture1, targetTexture);//将结果拷贝到目标RT
            rawImage.texture = targetTexture;
            RenderTexture.ReleaseTemporary(renderTexture1);//释放申请的两块RenderBuffer内容
            RenderTexture.ReleaseTemporary(renderTexture2);
        }

        //模糊尺寸 = 3 ，模糊采样=1 ，模糊次数 = 2
        public void RenderImageFastBlur()
        {
            if (rawTexture == null)
            {
                return;
            }

            //原图
            RenderTexture source = new RenderTexture(rawTexture.width, rawTexture.height, 0);
            Graphics.Blit(rawTexture, source, blurMaterial);
            //DestroyImmediate(rawTexture, true);
            RenderTexture targetTexture = source;
            
            float num = 1f / (1f * (float)(1 << this.downsample_fastBlur));
            this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize_fastBlur * num, -this.blurSize_fastBlur * num, 0f, 0f));
            source.filterMode = FilterMode.Bilinear;
            int width = source.width >> this.downsample_fastBlur;
            int height = source.height >> this.downsample_fastBlur;
            RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
            renderTexture.filterMode = FilterMode.Bilinear;
            Graphics.Blit(source, renderTexture, this.blurMaterial, 0);
            int num2 = 2;
            for (int i = 0; i < this.blurIterations_fastBlur; i++)
            {
                float num3 = (float)i * 1f;
                this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize_fastBlur * num + num3, -this.blurSize_fastBlur * num - num3, 0f, 0f));
                RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
                temporary.filterMode = FilterMode.Bilinear;
                Graphics.Blit(renderTexture, temporary, this.blurMaterial, 1 + num2);
                RenderTexture.ReleaseTemporary(renderTexture);
                renderTexture = temporary;

                temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
                temporary.filterMode = FilterMode.Bilinear;
                Graphics.Blit(renderTexture, temporary, this.blurMaterial, 2 + num2);
                RenderTexture.ReleaseTemporary(renderTexture);
                renderTexture = temporary;
            }
            Graphics.Blit(renderTexture, targetTexture);
            rawImage.texture = targetTexture;
            RenderTexture.ReleaseTemporary(renderTexture);
        }

        private void OnDisable()
        {
            if (rawImage)
            {
                rawTexture = null;
                rawImage.texture = rawTexture;
            }
        }

        private void OnValidate()
        {
            if (rawTexture != null)
            {
                this.UpdateMaterial();
                switch (textureBlurType)
                {
                    case TextureBlurType.FastBlur:
                        this.RenderImageFastBlur();
                        break;
                    case TextureBlurType.SampleBlur:
                        this.ApplyBlur();
                        break;
                }
            }
        }
    }

    public enum TextureBlurType
    {
        FastBlur,
        SampleBlur,
    }

}