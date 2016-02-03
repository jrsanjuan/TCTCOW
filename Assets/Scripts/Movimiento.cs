using UnityEngine;
using System.Collections;

public class Movimiento : MonoBehaviour {

	// Controls the movement
	bool moving;

	Vector3 vectorRotacion;
	float anguloInicial;
	float anguloFinal;
	Vector3 posicionInicial;
	Vector3 posicionFinal;
	Vector3 puntoRotacion;
	int signoRotacion;

	// Necesitamos almacenar las rotaciones que hemos ido realizando en el cubo
	Quaternion rotacionAcumulada;
	// Para saber la ultima tecla pulsada
	KeyCode lastHitKey;

	public float velocidadRotacion;
	
	void Start () {

		moving = false;
		posicionInicial = Vector3.zero;
		anguloInicial = 0f;
		rotacionAcumulada = Quaternion.AngleAxis(0, transform.up);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.name != "Start" && other.name != "Final") {
			AudioSource audio = GetComponent<AudioSource>();
			audio.Play();
			if (lastHitKey == KeyCode.UpArrow) {
				preMovement(KeyCode.DownArrow, new Vector3(0.0f,0.0f,1.0f), new Vector3(1.0f,0.0f,0.0f), -1, 
				            new Vector3(0.0f,-0.5f,-0.5f), new Vector3(0.0f,0.5f,-0.5f), new Vector3(0.0f,-0.5f,-0.5f));
			}
			else if (lastHitKey == KeyCode.DownArrow) {
				preMovement(KeyCode.UpArrow, new Vector3(0.0f,0.0f,1.0f), new Vector3(1.0f,0.0f,0.0f), 1, 
				            new Vector3(0.0f,-0.5f,0.5f), new Vector3(0.0f,0.5f,0.5f), new Vector3(0.0f,-0.5f,0.5f));
			}
			else if (lastHitKey == KeyCode.RightArrow) {
				preMovement(KeyCode.LeftArrow, new Vector3(1.0f,0.0f,0.0f), new Vector3(0.0f,0.0f,1.0f), 1, 
				            new Vector3(-0.5f,-0.5f,0.0f), new Vector3(-0.5f,0.5f,0.0f), new Vector3(-0.5f,-0.5f,0.0f));
			}
			else if (lastHitKey == KeyCode.LeftArrow) {
				preMovement(KeyCode.RightArrow, new Vector3(1.0f,0.0f,0.0f), new Vector3(0.0f,0.0f,1.0f), -1, 
				            new Vector3(0.5f,-0.5f,0.0f), new Vector3(0.5f,0.5f,0.0f), new Vector3(0.5f,-0.5f,0.0f));
			}
		}
		else if (other.name == "Final") {
			StartCoroutine("changeLevel");
		}
	}

	IEnumerator changeLevel() {
		float fadeTime = GameObject.FindWithTag("Nivel").GetComponent<Fading>().BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		if (Application.loadedLevel != 4)
			Application.LoadLevel(Application.loadedLevel + 1);
		else
			Application.LoadLevel(0);
	}

	/*void FixedUpdate() {
		// Se esta realizando un movimiento
		if (moving) {
			// La rotacion es dependiente de los FPS, a mas FPS, menos tardaremos en hacer la rotacion. 
			float deltaAngulo = velocidadRotacion;
			anguloInicial = anguloInicial + signoRotacion*deltaAngulo;
			
			if (Mathf.Abs(anguloInicial-anguloFinal) > 0f) {
				transform.RotateAround (puntoRotacion, signoRotacion * vectorRotacion, deltaAngulo);
			}
			else {
				// Corregimos la posicion y la rotacion final de cubo
				transform.rotation = rotacionAcumulada;
				transform.position = posicionFinal;
				moving = false;
			}
		}
	}*/

	void Update () {
		
		// Antes de realizar un movimiento
		if (!moving) {
			
			if (Input.GetKey(KeyCode.UpArrow)) {
				preMovement(KeyCode.UpArrow, new Vector3(0.0f,0.0f,1.0f), new Vector3(1.0f,0.0f,0.0f), 1, 
				            new Vector3(0.0f,-0.5f,0.5f), new Vector3(0.0f,0.5f,0.5f), new Vector3(0.0f,-0.5f,0.5f));
				lastHitKey = KeyCode.UpArrow;
			}
			else if (Input.GetKey(KeyCode.DownArrow)) {
				preMovement(KeyCode.DownArrow, new Vector3(0.0f,0.0f,1.0f), new Vector3(1.0f,0.0f,0.0f), -1, 
				            new Vector3(0.0f,-0.5f,-0.5f), new Vector3(0.0f,0.5f,-0.5f), new Vector3(0.0f,-0.5f,-0.5f));
				lastHitKey = KeyCode.DownArrow;
			}
			else if (Input.GetKey(KeyCode.RightArrow)) {
				preMovement(KeyCode.RightArrow, new Vector3(1.0f,0.0f,0.0f), new Vector3(0.0f,0.0f,1.0f), -1, 
				            new Vector3(0.5f,-0.5f,0.0f), new Vector3(0.5f,0.5f,0.0f), new Vector3(0.5f,-0.5f,0.0f));
				lastHitKey = KeyCode.RightArrow;
			}
			
			else if (Input.GetKey(KeyCode.LeftArrow)) {
				preMovement(KeyCode.LeftArrow, new Vector3(1.0f,0.0f,0.0f), new Vector3(0.0f,0.0f,1.0f), 1, 
				            new Vector3(-0.5f,-0.5f,0.0f), new Vector3(-0.5f,0.5f,0.0f), new Vector3(-0.5f,-0.5f,0.0f));
				lastHitKey = KeyCode.LeftArrow;
			}
		}
		// Se esta realizando un movimiento
		if (moving) {
			// La rotacion es dependiente de los FPS, a mas FPS, menos tardaremos en hacer la rotacion. 
			float deltaAngulo = velocidadRotacion;
			anguloInicial = anguloInicial + signoRotacion*deltaAngulo;
			
			if (Mathf.Abs(anguloInicial-anguloFinal) > 0f) {
				transform.RotateAround (puntoRotacion, signoRotacion * vectorRotacion, deltaAngulo);
			}
			else {
				// Corregimos la posicion y la rotacion final de cubo
				transform.rotation = rotacionAcumulada;
				transform.position = posicionFinal;
				moving = false;
			}
		}
	}

	void cubeUpFall (KeyCode key, Vector3 rot, bool falling) {
		// Ajustar los angulos para la subida y bajada de alturas
		if (key == KeyCode.UpArrow || key == KeyCode.LeftArrow)
			anguloFinal += 90;
		else
			anguloFinal -= 90;
		if (!falling)
			posicionFinal = posicionFinal + new Vector3(0.0f,1.0f,0.0f);
		else
			posicionFinal = posicionFinal - new Vector3(0.0f,1.0f,0.0f);
		puntoRotacion = posicionInicial + rot;
		rotacionAcumulada = Quaternion.AngleAxis(90, signoRotacion*vectorRotacion)*rotacionAcumulada;
	}
	
	void preMovement (KeyCode key, Vector3 vectorFinal, Vector3 vectorRot, int signoRot, Vector3 puntoRot, 
	                  Vector3 alturaRot, Vector3 caidaRot) {

		// Posiciones iniciales/finales del cubo
		posicionInicial = transform.TransformPoint(Vector3.zero);
		// Depende de las teclas pulsadas
		if (key == KeyCode.UpArrow || key == KeyCode.RightArrow)
			posicionFinal = posicionInicial + vectorFinal;
		else
			posicionFinal = posicionInicial - vectorFinal;
		
		// Cogemos todos los tiles por los que nos podemos mover. Tiene tag
		GameObject[] cubosALosQueMoverse = GameObject.FindGameObjectsWithTag("CuboEscenario"); 
		
		foreach(GameObject cubeMoving in cubosALosQueMoverse) {
			
			if (Mathf.Abs(cubeMoving.transform.position.x - posicionFinal.x) < 0.1 && 
			    Mathf.Abs(cubeMoving.transform.position.z - posicionFinal.z) < 0.1) 
			{
				// Activamos el movimiento
				cubeMoving.transform.GetComponent<MeshRenderer>().enabled = true;
				moving = true;
				
				// Definimos el vector sobre el que rota el cub
				vectorRotacion = vectorRot;
				signoRotacion = signoRot;

				// Definimos el angulo con el que rota el cubo
				anguloInicial = transform.eulerAngles.z;
				if (key == KeyCode.UpArrow || key == KeyCode.LeftArrow)
					anguloFinal = anguloInicial + 90;
				else
					anguloFinal = anguloInicial - 90;
				
				//Definimos el punto sobre el que realizamos la rotacion
				puntoRotacion = posicionInicial + puntoRot;
				
				// Acumulamos una rotacion de 90 grados
				rotacionAcumulada = Quaternion.AngleAxis(90, signoRotacion * vectorRotacion) * rotacionAcumulada;
				
				// Alturas
				// En este caso, y en el siguiente, tenemos que realizar una rotacion de 180 grados
				// Volvemos a acumular otros 90 grados de la rotacion
				if (Mathf.Abs (transform.position.y-cubeMoving.transform.position.y) < 0.1)
					cubeUpFall(key, alturaRot, false);
				else if (transform.position.y >= (2 + cubeMoving.transform.position.y))
					cubeUpFall(key, caidaRot, true);

				break;
			}
			
		}
	}
	
}
