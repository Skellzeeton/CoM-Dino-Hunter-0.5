using UnityEngine;
using UnityEngine.UI;

public class CameraFade : MonoBehaviour
{
    protected static GameObject cameraFade;
    protected static CameraFade fade;

    protected Color fadeInColor  = new Color(0.5f, 0.5f, 0.5f, 0f);
    protected Color fadeOutColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    protected float m_time;
    protected bool isfadein;
    protected bool isfadeout;
    protected float lasttime;

    protected RawImage fadeImage;

    public void Init()
    {
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999999;

        gameObject.AddComponent<CanvasScaler>();
        gameObject.AddComponent<GraphicRaycaster>();

        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(transform, false);

        fadeImage = imageObj.AddComponent<RawImage>();
        fadeImage.texture = CameraTexture(Color.black);
        fadeImage.color = fadeInColor;

        RectTransform rt = fadeImage.rectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    protected Texture2D CameraTexture(Color color)
    {
        Texture2D tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        tex.SetPixel(0, 0, color);
        tex.Apply();
        return tex;
    }

    public static void CameraFadeOut(float time)
    {
        Check();
        if (fade != null) fade.FadeOut(time);
    }

    public static void CameraFadeIn(float time)
    {
        Check();
        if (fade != null) fade.FadeIn(time);
    }

    public static void Clear()
    {
        if (cameraFade != null)
        {
            Object.Destroy(cameraFade);
            cameraFade = null;
            fade = null;
        }
    }

    protected static void Check()
    {
        if (cameraFade == null)
        {
            cameraFade = new GameObject("CameraFade");
            fade = cameraFade.AddComponent<CameraFade>();
            fade.Init();
        }
    }

    protected void FadeOut(float time)
    {
        m_time = Mathf.Max(0.01f, time);
        isfadeout = true;
        isfadein = false;
        lasttime = 0f;
    }

    protected void FadeIn(float time)
    {
        m_time = Mathf.Max(0.01f, time);
        isfadein = true;
        isfadeout = false;
        lasttime = 0f;
    }

    private void Update()
    {
        if ((isfadeout || isfadein) && fadeImage != null)
        {
            lasttime += Time.deltaTime;
            float t = Mathf.Clamp01(lasttime / m_time);

            if (isfadeout)
            {
                fadeImage.color = Color.Lerp(fadeInColor, fadeOutColor, t);
            }
            else if (isfadein)
            {
                fadeImage.color = Color.Lerp(fadeOutColor, fadeInColor, t);
            }

            if (lasttime >= m_time)
            {
                isfadein = false;
                isfadeout = false;
                lasttime = 0f;
                m_time = 0f;
            }
        }
    }
}