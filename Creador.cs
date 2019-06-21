using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using zom = NPC.Enemy;
using villa = NPC.Ally;
public class Creador : MonoBehaviour
{
    //declaracion publica para los textox que se van mostrar en pantalla
    public TextMeshProUGUI cantidadZombies;
    public TextMeshProUGUI cantidadVillagers;
    public int varZombies;
    public int varVillagers;
    public GameObject[] Zomb,alde;
    // Cada vez que se inica el juego crea los zombie, los aldeanos y el heroe
    void Start()
    {
        new CrearInstancias();
    }
    // Usamos el canvas para mostrar cuantos Zombies y Aldeanos existen en el momento
    private void Update()
    {
        Zomb = GameObject.FindGameObjectsWithTag("Zombie");
        alde = GameObject.FindGameObjectsWithTag("Villager");
        foreach (GameObject item in Zomb)
        {
            varZombies = Zomb.Length;
        }
        foreach (GameObject item in alde)
        {
            varVillagers = alde.Length;
        }

        if(alde.Length == 0)
        {
            cantidadVillagers.text = 0.ToString();
        }
        else
        {
            cantidadVillagers.text = varVillagers.ToString();
        }
        cantidadZombies.text = varZombies.ToString();
    }
}
// Creacion de cubos al azar y se les agrega al azar un componente ya sea para ser aldeano o para zombie y los pone en una posicion al azar
 class CrearInstancias 
{
    public GameObject cube;
    public readonly int minInstancias = Random.Range(5, 16);
    int escoger = 0;
    const int MAX = 26;
    public CrearInstancias()
    {
        for (int i = 0; i < Random.Range(minInstancias,MAX); i++)
        {
            if (escoger == 0)
            {
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.AddComponent<Camera>();
                cube.AddComponent<Controlador>();
                cube.AddComponent<Controlador.MirarH>();
                cube.AddComponent<Controlador.MoverH>();
                cube.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
                escoger += 1;
            }
            int selec = Random.Range(escoger, 3);
            if (selec == 1)
            {
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.AddComponent<villa.Ciudadanos>();
                cube.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
            }
            if (selec == 2)
            {
                cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.AddComponent<zom.Zombie>();
                cube.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
            }
        }
    }
}