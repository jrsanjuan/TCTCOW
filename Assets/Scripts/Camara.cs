using UnityEngine;
using System.Collections;

public class Camara : MonoBehaviour {
	
	float aceleracion = 0f;
	float rozamiento = 0.95f;
	float velocidad = 0f;
		
	void Update () {
		if(Input.GetKey(KeyCode.Space)){
			aceleracion += 0.01f;
			velocidad = velocidad + aceleracion;

			if(velocidad > 3f){
				velocidad = 3f;
			}
		}

		transform.RotateAround(new Vector3(3f,9f,3f),Vector3.up,velocidad);
		aceleracion *= rozamiento;
		velocidad *= rozamiento;
	}
}