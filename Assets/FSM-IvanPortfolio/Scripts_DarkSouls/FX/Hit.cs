/////////////////////////////////////////////////////////////////////
//Copyright © 2013-2016 Ivan Perez
//Author: Ivan Perez
//Linkedin: http://es.linkedin.com/in/ivanperezduran
//Portfolio: http://binarycv.com
/////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// Hit FX.
/// </summary>
public class Hit : MonoBehaviour {

    public float lifeTime = 0.25f;
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(LifeTimer());    
	}
	
	private IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
	}
}
