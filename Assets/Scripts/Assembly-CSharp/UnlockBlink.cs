using UnityEngine;

public class UnlockBlink : MonoBehaviour
{
	public GameObject go_blink;

	public TUIMeshSprite img_texture;

	private bool open_blink;

	private float fade_time = 0.5f;

	private float now_time;

	private void Start()
	{
	}

	private void Update()
	{
		if (open_blink)
		{
			now_time += Time.deltaTime;
			if (now_time < fade_time)
			{
				go_blink.transform.localScale = new Vector3(now_time * 4f, now_time * 4f, 1f);
			}
			go_blink.transform.localEulerAngles += new Vector3(0f, 0f, -1f);
		}
	}

	public void OpenBlink(TUIMeshSprite m_sprite)
	{
		open_blink = true;
		base.transform.localPosition = new Vector3(0f, 0f, base.transform.localPosition.z);
		img_texture.UseCustomize = true;
		img_texture.CustomizeTexture = m_sprite.CustomizeTexture;
		img_texture.CustomizeRect = m_sprite.CustomizeRect;
		img_texture.GetComponent<Animation>().Play();
	}

	public void OpenBlink(string m_texture_name)
	{
		open_blink = true;
		img_texture.texture = m_texture_name;
		base.transform.localPosition = new Vector3(0f, 0f, base.transform.localPosition.z);
		img_texture.GetComponent<Animation>().Play();
	}

	public void CloseBlink()
	{
		open_blink = false;
		now_time = 0f;
		base.transform.localPosition = new Vector3(0f, -1000f, base.transform.localPosition.z);
		img_texture.UseCustomize = false;
		img_texture.CustomizeTexture = null;
		img_texture.CustomizeRect = new Rect(0f, 0f, 0f, 0f);
	}
}
