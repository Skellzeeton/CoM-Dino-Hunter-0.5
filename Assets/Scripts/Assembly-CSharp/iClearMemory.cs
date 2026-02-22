using System;
using System.Collections;
using UnityEngine;

public class iClearMemory : MonoBehaviour
{
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void ClearMemory()
	{
		StartCoroutine(Clear());
	}

	protected IEnumerator Clear()
	{
		Debug.Log("Clear");
		GC.Collect();
		yield return Resources.UnloadUnusedAssets();
	}
}
