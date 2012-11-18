using UnityEngine;
using System.Collections;

public class Lane : MonoBehaviour
{
	private bool _on;
	
	void Start()
	{
		//renderer.enabled = true;
		renderer.material.color = new Color(0, 1, 1, 0.5f);
	}
	
	void OnMouseOver()
	{
		if (!_on && Input.GetMouseButton(0))
		{
			_on = true;
			renderer.enabled = true;
			//StartCoroutine(FadeIn());
		}
		else if (!Input.GetMouseButton(0))
		{
			_on = false;
			renderer.enabled = false;
		}
	}
	
	void OnMouseExit()
	{
		if (_on)
		{
			_on = false;
			renderer.enabled = false;
			//StartCoroutine(FadeOut());
		}
	}
	
	IEnumerator FadeOut()
	{
		for (float t = 0.5f; t > 0; t -= Time.deltaTime * 5)
		{
			renderer.material.color = new Color(0, 1, 1, t);
			yield return null;
		}
		renderer.material.color = new Color(0, 1, 1, 0);
	}
	
	IEnumerator FadeIn()
	{
		for (float t = 0; t < 0.5f; t += Time.deltaTime * 5)
		{
			renderer.material.color = new Color(0, 1, 1, t);
			yield return null;
		}
		renderer.material.color = new Color(0, 1, 1, 1);
	}
}
