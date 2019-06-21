// Estructuras publica para el Zombie 
public struct InformaciondelZombie
{
    public string gusto;
    public string nombre;
    public int edad;
}
 // Estructura publica para el ciudadano
public struct InformaciondelCiudadano
{
    public string nombre;
    public int edad;
    static public implicit operator InformaciondelZombie(InformaciondelCiudadano c)
    {
        InformaciondelZombie z = new InformaciondelZombie();
        z.gusto = "Cerebros";
        z.edad = c.edad;
        z.nombre = "Zombie " + c.nombre;
        return z;
    }
}
