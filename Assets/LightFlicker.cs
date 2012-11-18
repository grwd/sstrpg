using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
	[SerializeField]
	public Flicker[] Flickers;
	
	private Light _light;
	private int _flickerCounter = 0;
	private float _flickerTimer = 0f;
	
	void Start ()
	{
		_light = GetComponent<Light>();
	}
	
	void Update ()
	{
		
		_flickerTimer += Time.deltaTime;
		
		if (_flickerTimer > Flickers[_flickerCounter].Delay)
		{
			_flickerTimer -= Flickers[_flickerCounter].Delay;
			
			_light.intensity = Flickers[_flickerCounter].Intensity;
			_flickerCounter++;
			if (_flickerCounter == Flickers.Length)
				_flickerCounter = 0;
		}
	}
}
	
[System.Serializable]
public class Flicker
{
	public float Delay;
	public float Intensity;
}