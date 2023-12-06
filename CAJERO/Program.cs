using System;
using System.Collections.Generic;

class Usuario
{
    private static readonly Random random = new Random();

    public string NumeroCuenta { get; set; }
    public string NombreCompleto { get; set; }
    public string NumeroCedula { get; set; }

    private string _contraseña;
    public decimal DineroTotal { get; set; }
    public decimal LimiteDiarioRetiro { get; set; }
    public decimal LimiteDiarioRecarga { get; set; }

    private int _puntosViveColombia;

    public int PuntosViveColombia
    {
        get { return _puntosViveColombia; }
        set { _puntosViveColombia = value; }
    }

    private static HashSet<string> cedulasRegistradas = new HashSet<string>();
    public static bool VerificarCedulaUnica(string cedula)
    {
        return cedulasRegistradas.Add(cedula);
    }

    public Usuario(string nombreCompleto, string numeroCedula, string contraseña)
    {
        NumeroCuenta = GenerarNumeroCuenta();
        NombreCompleto = nombreCompleto;
        NumeroCedula = numeroCedula;
        Contraseña = contraseña;
        DineroTotal = 0;
        LimiteDiarioRetiro = 2000000; // Límite inicial para retiros diarios
        LimiteDiarioRecarga = 5000000; // Límite para recargas diarias
    }

    private string GenerarNumeroCuenta()
    {
        return random.Next(100000, 999999).ToString();
    }

    public string Contraseña
    {
        get { return _contraseña; }
        set
        {
            try
            {
                if (value.Length == 5)
                {
                    _contraseña = value;
                }
                else
                {
                    throw new Exception("La contraseña debe tener exactamente 5 caracteres.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    public void MostrarInformacion()
    {
        Console.WriteLine("---------------------");
        Console.WriteLine($"Nombre   | {NombreCompleto}");
        Console.WriteLine($"Cedula   | {NumeroCedula}");
        Console.WriteLine($"Numero de cuenta | {NumeroCuenta}");
        Console.WriteLine($"Dinero total $ | {DineroTotal}");
        Console.WriteLine($"Puntos ViveColombia | {_puntosViveColombia}");
        Console.WriteLine("---------------------");
    }

    public void Retirar(decimal cantidad)
    {
        if (cantidad <= DineroTotal && cantidad <= LimiteDiarioRetiro)
        {
            DineroTotal -= cantidad;
            LimiteDiarioRetiro -= cantidad;

            Console.Clear();
            Console.WriteLine($"Retiro exitoso. Dinero restante: {DineroTotal}");
            Console.WriteLine($"Monto disponible para retirar hoy: {LimiteDiarioRetiro}");
        }
        else
        {
            Console.Clear();
            Console.WriteLine("No se puede realizar el retiro. Verifique el saldo y el límite diario.");
        }
    }

    public void Transferir(Usuario destinatario, decimal monto)
    {
        if (monto <= DineroTotal)
        {
            DineroTotal -= monto;

            destinatario.DineroTotal += monto;

            int puntosGanados = (int)(monto / 100000) * 2;
            _puntosViveColombia += puntosGanados;

            Console.Clear();
            Console.WriteLine($"Transferencia exitosa a {destinatario.NombreCompleto}. Dinero restante: {DineroTotal}");
            Console.WriteLine($"¡Has ganado {puntosGanados} Puntos ViveColombia por esta transferencia!");
        }
        else
        {
            Console.Clear();
            Console.WriteLine("No se puede realizar la transferencia. Verifique el saldo o el monto de transferencia.");
        }
    }


    public void Recargar(decimal monto)
    {
        if (monto <= LimiteDiarioRecarga)
        {
            DineroTotal += monto;
            LimiteDiarioRecarga -= monto;

            int puntosGanados = (int)(monto / 150000) * 5;
            _puntosViveColombia += puntosGanados;

            Console.Clear();
            Console.WriteLine($"Recarga exitosa. Dinero total: {DineroTotal}");
            Console.WriteLine($"Monto disponible para recargar hoy: {LimiteDiarioRecarga}");
            Console.WriteLine($"¡Has ganado {puntosGanados} Puntos ViveColombia por esta recarga!");
        }
        else
        {
            Console.Clear();
            Console.WriteLine("No se puede recargar más de 5 millones diarios.");
        }
    }

    public void CanjearPuntos(int cantidadPuntos)
    {
        if (cantidadPuntos <= _puntosViveColombia)
        {
            _puntosViveColombia -= cantidadPuntos;
            decimal valorCanje = cantidadPuntos * 7;
            DineroTotal += valorCanje;

            Console.Clear();
            Console.WriteLine($"Canje exitoso. Se han sumado {valorCanje} pesos a tu cuenta.");
            Console.WriteLine($"Puntos ViveColombia restantes: {_puntosViveColombia}");
        }
        else
        {
            Console.Clear();
            Console.WriteLine("No tienes suficientes Puntos ViveColombia para realizar el canje.");
        }
    }
}

class Program
{
    static List<Usuario> usuarios = new List<Usuario>();

    static void Main()
    {

        while (true)
        {
            
            Console.WriteLine("¡Bienvenido a ViveColombia!");
            Console.WriteLine("1. Iniciar sesión");
            Console.WriteLine("2. Salir");
            Console.Write("Seleccione una opción: ");
            if (int.TryParse(Console.ReadLine(), out int opcion))
            {
                switch (opcion)
                {
                    case 1:
                        IniciarSesion();
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Gracias por usar ViveColombia. ¡Hasta luego!");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Por favor, seleccione una opción válida.");
                        break;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Entrada inválida. Introduce un número.");
            }
        }
    }

        static void IniciarSesion()
    {
        Console.WriteLine("Si no tiene una cuenta precione enter para continuar");
        Console.WriteLine("Ingrese el número de cuenta: ");
        string numeroCuenta = Console.ReadLine();

        Usuario usuario = usuarios.Find(u => u.NumeroCuenta == numeroCuenta);

        if (usuario == null)
        {
            Console.WriteLine("Cuenta no encontrada.");
            Console.Write("¿Desea crear una cuenta? (s/n): ");
            if (Console.ReadLine().ToLower() == "s")
            {
                CrearCuenta();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Volviendo al menú principal.");
            }
        }
        else
        {
            Console.Write("Ingrese la contraseña: ");
            string contraseña = Console.ReadLine();

            if (contraseña == usuario.Contraseña)
            {
                MenuPrincipal(usuario);
            }
            else
            {
                Console.WriteLine("Contraseña incorrecta. Volviendo al menú principal.");
            }
        }
    }

    static void CrearCuenta()
    {
        Console.Write("Ingrese su nombre completo: ");
        string nombreCompleto = Console.ReadLine();

        string numeroCedula;
        while (true)
        {
            Console.Write("Ingrese su número de cédula: ");
            numeroCedula = Console.ReadLine();

            if (Usuario.VerificarCedulaUnica(numeroCedula))
            {
                break;
            }
            else
            {
                Console.WriteLine("Ya existe una cuenta con esta cédula. Por favor, ingrese otra cédula.");
            }
        }

        string contraseña = "";
        while (contraseña.Length != 5)
        {
            Console.Write("Ingrese su contraseña (debe tener 5 caracteres): ");
            contraseña = Console.ReadLine();
        }

        Usuario nuevoUsuario = new Usuario(nombreCompleto, numeroCedula, contraseña);
        usuarios.Add(nuevoUsuario);

        Console.Clear();
        Console.WriteLine($"Cuenta creada con éxito. Su número de cuenta es: {nuevoUsuario.NumeroCuenta}");
        Console.WriteLine("Volviendo al menú principal.");
    }


    static void MenuPrincipal(Usuario usuario)
    {
        Console.Clear();
        while (true)
        {
            Console.WriteLine("-------- Menú Principal --------");
            usuario.MostrarInformacion();
            Console.WriteLine("1. Retirar");
            Console.WriteLine("2. Transferencias");
            Console.WriteLine("3. Recargar");
            Console.WriteLine("4. Canjear Puntos");
            Console.WriteLine("5. Comprar con Puntos ViveColombia");
            Console.WriteLine("6. Cerrar sesión");
            Console.Write("Seleccione una opción: ");
            int opcion = int.Parse(Console.ReadLine());

            switch (opcion)
            {
                case 1:
                    Console.Write("Ingrese la cantidad a retirar: ");
                    decimal cantidadRetiro = decimal.Parse(Console.ReadLine());
                    usuario.Retirar(cantidadRetiro);
                    break;
                case 2:
                    Console.Write("Ingrese el número de cuenta del destinatario: ");
                    string numeroCuentaDestinatario = Console.ReadLine();
                    Usuario destinatario = usuarios.Find(u => u.NumeroCuenta == numeroCuentaDestinatario);

                    if (destinatario == null)
                    {
                        Console.WriteLine("Cuenta de destinatario no encontrada. Verifique el número de cuenta.");
                    }
                    else
                    {
                        Console.Write("Ingrese el monto a transferir: ");
                        decimal montoTransferencia = decimal.Parse(Console.ReadLine());
                        usuario.Transferir(destinatario, montoTransferencia);
                    }
                    break;
                case 3:
                    Console.Write("Ingrese el monto a recargar: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal montoRecarga))
                    {
                        usuario.Recargar(montoRecarga);
                    }
                    else
                    {
                        Console.WriteLine("Cantidad inválida. Introduce un número decimal válido.");
                    }
                    break;
                case 4:
                    Console.Write("Ingrese la cantidad de puntos a canjear: ");
                    int cantidadPuntos = int.Parse(Console.ReadLine());
                    usuario.CanjearPuntos(cantidadPuntos);
                    break;
                case 5:
                    MostrarMenuCompras(usuario);
                    break;
                case 6:
                    Console.Clear();
                    Console.WriteLine("Sesión cerrada. ¡Vuelve pronto a ViveColombia!");
                    return;
                default:
                    Console.WriteLine("Opción no válida. Por favor, seleccione una opción válida.");
                    break;
            }
        }
    }
    static void MostrarMenuCompras(Usuario usuario)
    {
        Console.Clear();
        Console.WriteLine("-------- Menú de Compras con Puntos ViveColombia --------");
        usuario.MostrarInformacion();
        Console.WriteLine("Puedes comprar los siguientes servicios con tus Puntos ViveColombia:");
        Console.WriteLine("1. 1 mes de Spotify (35 puntos)");
        Console.WriteLine("2. 1 mes de Xbox Game Pass (60 puntos)");
        Console.WriteLine("3. 1 mes de Apple TV (40 puntos)");
        Console.WriteLine("4. Una hamburguesa del McDonald's (50 puntos)");
        Console.WriteLine("5. 1 mes de Netflix (30 puntos)");
        Console.WriteLine("0. Volver al Menú Principal");
        Console.Write("Seleccione una opción: ");
        int opcionCompra = int.Parse(Console.ReadLine());

        switch (opcionCompra)
        {
            case 1:
                RealizarCompra(usuario, 35, "1 mes de Spotify");
                break;
            case 2:
                RealizarCompra(usuario, 60, "1 mes de Xbox Game Pass");
                break;
            case 3:
                RealizarCompra(usuario, 40, "1 mes de Apple TV");
                break;
            case 4:
                RealizarCompra(usuario, 50, "Una hamburguesa del McDonald's");
                break;
            case 5:
                RealizarCompra(usuario, 30, "1 mes de Netflix");
                break;
            case 0:
                break;
            default:
                Console.WriteLine("Opción no válida. Por favor, seleccione una opción válida.");
                break;
        }
    }

    static void RealizarCompra(Usuario usuario, int puntosRequeridos, string servicio)
    {
        if (usuario.PuntosViveColombia >= puntosRequeridos)
        {
            usuario.PuntosViveColombia -= puntosRequeridos;
            Console.WriteLine($"¡Compra realizada con éxito! Has canjeado {puntosRequeridos} Puntos ViveColombia por '{servicio}'.");
            Console.WriteLine($"Puntos ViveColombia restantes: {usuario.PuntosViveColombia}");
        }
        else
        {
            Console.WriteLine("No tienes suficientes Puntos ViveColombia para realizar esta compra.");
        }

        Console.WriteLine("Presiona Enter para volver al Menú de Compras...");
        Console.ReadLine();
    }
}
