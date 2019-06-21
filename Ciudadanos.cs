using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zom = NPC.Enemy;
//Declaracion del namespace NPC 
namespace NPC
{
    //Declaracion del nameSpace Ally que esta dentro del namespace mayor NPC 
    namespace Ally
    {
        // Declaraciones necesarias para ciudadano como la informacion del script Datos a utilizar el estado si ya se volvio un zombi o no y la distacia
        // a recorrer del ciudadno y al informacion basica de este  
        public class Ciudadanos : MonoBehaviour
        {
            public InformaciondelCiudadano informacionAldeano = new InformaciondelCiudadano();
            public Estado estadoVillager;
            float tiempo;
            public float distancia;
            public float velocidadCorrer;
            public float edad;
            int D;
            public bool velocidadEstado = false;
            bool mirar = false;
            public Vector3 direccion;
            GameObject Target;
            GameObject[] villagers;
            //Declarar un enum de nombre para los ciudadanos
            public enum nombresciudadanos
            {
                Tomas, Diego, Marlon, Emmanuel, Maicol, Stefany, Vicky, Kira, Daniel, Drossnilo, Santiago, Kevin, Jorge, Anderson, Luis, Mateo, Daniela, Lucas, Hernan, Paula
            }
            // estados variados del ciudadano a utilizar
            public enum Estado
            {
                Idle, Moving, Rotating, Running
            }
            // Corrotina que hace correr a los aldeanos al estar en monos de 5 unidades de un zombie y lo hace en sentido contrario al zombie
            IEnumerator buscaZombies()
            {
                villagers = GameObject.FindGameObjectsWithTag("Zombie");
                foreach (GameObject item in villagers)
                {
                    zom.Zombie componenteZombie = item.GetComponent<zom.Zombie>();
                    if (componenteZombie != null)
                    {
                        distancia = Mathf.Sqrt(Mathf.Pow((item.transform.position.x - transform.position.x), 2) + Mathf.Pow((item.transform.position.y - transform.position.y), 2) + Mathf.Pow((item.transform.position.z - transform.position.z), 2));
                        if (!velocidadEstado)
                        {
                            if (distancia < 5f)
                            {
                                estadoVillager = Estado.Running;
                                Target = item;
                                velocidadEstado = true;
                            }
                        }
                    }
                }
                // si el ciudadano esta a mas de 5 unidades del zombie vuelve otra vez a estos aleatorios
                if (velocidadEstado)
                {
                    if (distancia > 5f)
                    {
                        velocidadEstado = false;
                    }
                }
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(buscaZombies());
            }
            //Se le agrega toda la informacion necesaria al ciudadano
            void Start()
            {
                Rigidbody Villa;
                this.gameObject.tag = "Villager";
                Villa = this.gameObject.AddComponent<Rigidbody>();
                Villa.constraints = RigidbodyConstraints.FreezeAll;
                Villa.useGravity = false;
                nombresciudadanos nombre;
                nombre = (nombresciudadanos)Random.Range(0, 20);
                informacionAldeano.nombre = nombre.ToString();
                edad = (int)Random.Range(15, 101);
                informacionAldeano.edad = (int)edad;
                velocidadCorrer = 10 / edad;
                this.gameObject.name = nombre.ToString();
                StartCoroutine(buscaZombies());
            }
            // Se le aplica una random entre estados para que no este siempre en un mismo estado
            void Update()
            {
                tiempo += Time.deltaTime;
                if (!velocidadEstado)
                {
                    if (tiempo >= 3)
                    {
                        D = Random.Range(0, 3);
                        mirar = true;
                        tiempo = 0;
                        if (D == 0)
                        {
                            estadoVillager = Estado.Idle;
                        }
                        else if (D == 1)
                        {
                            estadoVillager = Estado.Moving;
                        }
                        else if (D == 2)
                        {
                            estadoVillager = Estado.Rotating;

                        }
                    }
                }
                // declarando que hacer en cada caso de los estados del ciudadno si un estado es verdadero hacer las acciones requeridas
                switch (estadoVillager)
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

                    case Estado.Running:
                        direccion = Vector3.Normalize(Target.transform.position - transform.position);
                        transform.position -= direccion * velocidadCorrer;
                        break;
                }
            }
        }
    }
}

