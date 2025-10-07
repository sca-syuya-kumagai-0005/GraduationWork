using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class PlayRateTest : MonoBehaviour
{	
	[SerializeField] private VisualEffect _visualEffect;
	[SerializeField] private float _rate = 3f;
	private void Start()
	{
		_visualEffect = GetComponent<VisualEffect>();
	}

	private void Update()
	{
		_visualEffect.playRate = _rate;
	}
}
