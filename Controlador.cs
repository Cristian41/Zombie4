using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using zom = NPC.Enemy;
using villa = NPC.Ally;
// Declaracion de valor de la distancia del ciudadano textos en pantalla y declaracion de los aldeanos y zombies como un array de gameobjects
public class Controlador : MonoBehaviour
{
    float distanciaInical;
    float distanciaFinal;
    public float tiempo;
    public TextMeshProUGUI TextodelZombie;
    public TextMeshProUGUI TextodelVillager;
    GameObject[] aldeanos, zombie;
    // declarando la informacion del script Datos del zombie y del ciudadano como nuevo para poder utilizarlos en este Script
    InformaciondelCiudadano informacionAldeano = new InformaciondelCiudadano();
    InformaciondelZombie informacionZombie = new InformaciondelZombie();
    // Le pasamos la informacion necesaria a nuestro Héroe
    void Start()
    {
        Rigidbody heroe = this.gameObject.AddComponent<Rigidbody>();
        this.gameObject.tag = "Heroe";
        this.gameObject.name = "Heroe";
        heroe.constraints = RigidbodyConstraints.FreezeAll;
        heroe.useGravity = false;
        StartCoroutine(Sateliteorbital());
        TextodelZombie = GameObject.FindGameObjectWithTag("TextZombie").GetComponent<TextMeshProUGUI>();
        TextodelVillager = GameObject.FindGameObjectWithTag("TextAldeano").GetComponent<TextMeshProUGUI>();
    }
    //Declarando un contador de tiempo  
    public void Update()
    {
        tiempo += Time.fixedDeltaTime;
    }
    // Visualizacion sobre el recorrido que toma los zombie y los aldeanos
    
    IEnumerator Sateliteorbital()
    {
        zombie = GameObject.FindGameObjectsWithTag("Zombie");
        aldeanos = GameObject.FindGameObjectsWithTag("Villager");
        // Informacion necesario sobre el aldeano
        foreach (GameObject item in aldeanos)
        {
            yield return new WaitForEndOfFrame();
            villa.Ciudadanos componenteAldeano = item.GetComponent<villa.Ciudadanos>();
            if (componenteAldeano != null)
            {              
                distanciaInical = Mathf.Sqrt(Mathf.Pow((item.transform.position.x - transform.position.x), 2) + Mathf.Pow((item.transform.position.y - transform.position.y), 2) + Mathf.Pow((item.transform.position.z - transform.position.z), 2));
                if (distanciaInical < 5f)
                {
                    tiempo = 0;
                    informacionAldeano = item.GetComponent<villa.Ciudadanos>().informacionAldeano;
                    TextodelVillager.text = "Hola soy un " + informacionAldeano.nombre + " y he cumpido " + informacionAldeano.edad.ToString() + " años";
                }
                if (tiempo > 3)
                {
                    TextodelVillager.text = " ";
                }
            }
        }
        //Datos necesarios sobre el Zombie.
        foreach (GameObject itemZ in zombie)
        {
            yield return new WaitForEndOfFrame();
            zom.Zombie componenteZombie = itemZ.GetComponent<zom.Zombie>();
            if (componenteZombie != null)
            {              
                distanciaFinal = Mathf.Sqrt(Mathf.Pow((itemZ.transform.position.x - transform.position.x), 2) + Mathf.Pow((itemZ.transform.position.y - transform.position.y), 2) + Mathf.Pow((itemZ.transform.position.z - transform.position.z), 2));
                if (distanciaFinal < 5f)
                {
                    tiempo = 0;
                    informacionZombie = itemZ.GetComponent<zom.Zombie>().informacionZombie;
                    TextodelZombie.text = "Grrrrrrrrrrrr Comida, comidaaaaa Grrr " + informacionZombie.gusto;
                }
                if (tiempo > 3)
                {
                    TextodelZombie.text = " ";
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Sateliteorbital());
    }
    //Con las teclas W y S podemos mover al frente y atras a nuestro heroe con una velocidad aleatoria
    public class MoverH : MonoBehaviour
    {

        velocidad velocidad;

        private void Start()
        {
            velocidad  = new velocidad(Random.Range(0.25f, 0.5f));
        }
        private void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                this.gameObject.transform.Translate(0, 0, velocidad.velo);
            }
            if (Input.GetKey(KeyCode.S))
            {
                this.gameObject.transform.Translate(0, 0, -velocidad.velo);
            }
        }
    }
    //con las teclas A y D nos permite rotar al heroe A la izquierda y Derecha con una velocidad definida
    public class MirarH : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                this.gameObject.transform.Rotate(0, -3, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                this.gameObject.transform.Rotate(0, 3, 0);
            }
        }
    }
}
// Clase publica readonly para la velocidad aleatoria del heroe
public class velocidad
{
    public readonly float velo;
    public velocidad(float vel)
    {
        velo = vel;
    }
}
