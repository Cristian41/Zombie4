using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using villa = NPC.Ally;
//Declaracion del namespace NPC 
namespace NPC
{
    //Declaracion del nameSpace enemy que esta dentro del namespace mayor NPC 
    namespace Enemy
    {
        // declaraciones para el zombie como la informacion el color gusto y las demas cosas necesarias para el zombie
        public class Zombie : MonoBehaviour
        {
            public InformaciondelZombie informacionZombie;
            bool enemigoss = false;
            int color;
            public string gusto;
            public int D = 0;
            public float edad = 0;
            public float tiempo = 0;
            public bool mirar = false;
            public float velocidadseguir;
            public Estado estadodelzombie;
            public Vector3 direccion;
            float distanciaincial;
            float distanciafinal;
            public bool followState = false;
            GameObject objetivo, heroe;
            GameObject[] aldeanos;
            // Creacion de una corrutina que pone al zombie a buscar un objetivo y si esta dentro de 5 unidades del zombie lo persigue priorizando a los aldeanos
            IEnumerator buscaAldeanos()
            {
                heroe = GameObject.FindGameObjectWithTag("Hero");
                aldeanos = GameObject.FindGameObjectsWithTag("Villager");
                foreach (GameObject item in aldeanos)
                {
                    yield return new WaitForEndOfFrame();
                    villa.Ciudadanos componenteAldeano = item.GetComponent<villa.Ciudadanos>();
                    if (componenteAldeano != null)
                    {
                        distanciafinal = Mathf.Sqrt(Mathf.Pow((heroe.transform.position.x - transform.position.x), 2) + Mathf.Pow((heroe.transform.position.y - transform.position.y), 2) + Mathf.Pow((heroe.transform.position.z - transform.position.z), 2));
                        distanciaincial = Mathf.Sqrt(Mathf.Pow((item.transform.position.x - transform.position.x), 2) + Mathf.Pow((item.transform.position.y - transform.position.y), 2) + Mathf.Pow((item.transform.position.z - transform.position.z), 2));
                        if (!followState)
                        {

                            if(distanciaincial < 5f)
                            {
                                estadodelzombie = Estado.Pursuing;
                                objetivo = item;
                                followState = true;
                            }
                            else if (distanciafinal < 5f)
                            {
                                estadodelzombie = Estado.Pursuing;
                                objetivo = heroe;
                                followState = true;
                            }
                        }
                        if (distanciaincial < 5f && distanciafinal < 5f)
                        {
                            objetivo = item;
                        }
                    }
                }
                if (followState)
                {
                    if (distanciaincial > 5f && distanciafinal > 5f)
                    {
                        followState = false;
                    }
                }
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(buscaAldeanos());
            }
            // Partes del cuerpo a comer por zombie
            public enum Sabor
            {
                Pies, Cesos, Cerebro, Brazos, Corazón
            }
            //Diferentes estados del zombie
            public enum Estado
            {
                Moving, Idle, Rotating, Pursuing
            }
            // Se le otorga toda la información necesaria al zombie
            void Start()
            {
                if (!enemigoss)
                {
                    edad = (int)Random.Range(15, 101);
                    informacionZombie = new InformaciondelZombie();
                    color = Random.Range(0, 3);
                    Rigidbody Zom;
                    Zom = this.gameObject.AddComponent<Rigidbody>();
                    Zom.constraints = RigidbodyConstraints.FreezeAll;
                    Zom.useGravity = false;
                    this.gameObject.name = "Zombie";
                }
                else
                {
                    edad = informacionZombie.edad;
                    this.gameObject.name = informacionZombie.nombre;
                }
                StartCoroutine(buscaAldeanos());
                velocidadseguir = 10 / edad;
                this.gameObject.tag = "Zombie";
                Sabor Sabor;
                Sabor = (Sabor)Random.Range(0, 5);
                gusto = Sabor.ToString();
                informacionZombie.gusto = gusto;
                if (color == 0)
                {
                    this.gameObject.GetComponent<Renderer>().material.color = UnityEngine.Color.cyan;
                }
                if (color == 1)
                {
                    this.gameObject.GetComponent<Renderer>().material.color = UnityEngine.Color.magenta;
                }
                if (color == 2)
                {
                    this.gameObject.GetComponent<Renderer>().material.color = UnityEngine.Color.green;
                }
            }
            //Se le aplica una random entre estados para que no este siempre en un mismo estado
            void Update()
            {
                tiempo += Time.deltaTime;
                if (!followState)
                {
                    if (tiempo >= 3)
                    {
                        D = Random.Range(0, 3);
                        mirar = true;
                        tiempo = 0;
                        if (D == 0)
                        {
                            estadodelzombie = Estado.Idle;
                        }
                        else if (D == 1)
                        {
                            estadodelzombie = Estado.Moving;
                        }
                        else if (D == 2)
                        {
                            estadodelzombie = Estado.Rotating;
                        }
                    }
                }
                //declarando que hacer en cada caso de los estados del ciudadno si un estado es verdadero hacer las acciones requeridas
                switch (estadodelzombie)
                {
                    case Estado.Idle:
                        break;
                    case Estado.Moving:
                        if (mirar)
                        {
                            this.gameObject.transform.Rotate(0, Random.Range(0, 361), 0);
                        }
                        this.gameObject.transform.Translate(0, 0, 0.05f);
                        mirar = false;
                        break;
                    case Estado.Rotating:
                        this.gameObject.transform.Rotate(0, Random.Range(1, 50), 0);
                        break;
                    case Estado.Pursuing:
                        direccion = Vector3.Normalize(objetivo.transform.position - transform.position);
                        transform.position += direccion * velocidadseguir;
                        break;
                }
            }
            // Una colisión para cada vez que toca a un aldeano se convierta en un zombie y le agrega los script del zombie para se comporte como tal
            // y si colisiona con el heroe se acaba el juego
            private void OnCollisionEnter(Collision collision)
            {
                if (collision.gameObject.tag == "Villager")
                {
                    collision.gameObject.AddComponent<Zombie>().informacionZombie = collision.gameObject.GetComponent<NPC.Ally.Ciudadanos>().informacionAldeano;
                    collision.gameObject.GetComponent<Zombie>().enemigoss = true;
                    Destroy(collision.gameObject.GetComponent<NPC.Ally.Ciudadanos>());
                }
                if (collision.gameObject.tag == "Hero")
                {
                    SceneManager.LoadScene(0);
                }
            }
        }
    }
}