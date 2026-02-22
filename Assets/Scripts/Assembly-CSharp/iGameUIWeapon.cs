using UnityEngine;

public class iGameUIWeapon : MonoBehaviour
{
	protected UISprite m_Icon;

	private void Awake()
	{
		Transform transform = base.transform.Find("icon");
		if (transform != null)
		{
			m_Icon = transform.GetComponent<UISprite>();
			transform.localPosition -= new Vector3(0f, 0f, -0.1f);
		}
	}

	private void Update()
	{
	}

	public void SetIcon(string str)
	{
		if (!(m_Icon == null))
		{
			GameObject gameObject = PrefabManager.Get("Artist/Atlas/Weapon/" + str);
			if (gameObject != null)
			{
				m_Icon.atlas = gameObject.GetComponent<UIAtlas>();
			}
		}
	}
}
