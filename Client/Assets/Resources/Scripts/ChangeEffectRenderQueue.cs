using UnityEngine;
using System.Collections;

public class ChangeEffectRenderQueue : MonoBehaviour {

	public int renderQueue = 3000;
	public bool runOnlyOnce = false;
	
	void Start()
	{
		Update();
	}

	void Update()
	{
		Renderer[] theRenderer = GetComponentsInChildren<Renderer>();
		for(int i = 0; i < theRenderer.Length; ++i)
		{
			if(theRenderer[i] != null && theRenderer[i].sharedMaterial != null)
			{
				theRenderer[i].sharedMaterial.renderQueue = renderQueue;
			}
		}

		if(runOnlyOnce && Application.isPlaying)
		{
			this.enabled = false;
		}
	
	}
}
