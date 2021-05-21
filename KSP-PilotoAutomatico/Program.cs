using KRPC.Client;
using KRPC.Client.Services.KRPC;
using KSP_PilotoAutomatico.Actions;
using KSP_PilotoAutomatico.Manobras;
using System;
using System.Net;

class Program
{
    public static void Main()
    {
        IPAddress serverAddress = IPAddress.Parse("127.0.0.1");
        Connection connection;

        try
        {
            connection = new Connection(
                   name: "Main Connection",
                   address: serverAddress);
        }
        catch (Exception ex)
        {
            string m = "Ocorreu um erro ao se conectar ao serviço do KRCP.";
            Console.Error.Write(m + ex.Message);
            return;
        }

        var krpc = connection.KRPC();

        Console.WriteLine("Connexão bem sucedida");
        Console.WriteLine(krpc.GetStatus().Version);

        int? opt;

        opt = Funcoes.GetOpt();
        Console.WriteLine("Opcao Selecionada: " + opt);

        while (opt != 0 && opt != null)
        {
            try
            {
                connection = new Connection("Main Connection", serverAddress);

                switch (opt)
                {
                    case 1:
                        Console.WriteLine("Iniciando sequencia sub orbital");
                        SubOrbitalFlight.Start(connection);
                        break;

                    case 2:
                        Console.WriteLine("Iniciando lançamento orbital");
                        DecolagemOrbital.Start(connection);
                        break;
                    case 3:
                        Console.WriteLine("Manobrando");
                        Manobrar.Start(connection);
                        break;
                    case 4:
                        Console.WriteLine("Pouso");
                        Pouso.Start(connection);
                        break;
                    default:
                        Console.WriteLine("Saindo");
                        break;

                }

                connection.Dispose();
            }
            catch (Exception ex)
            {
                Console.Beep();
                Console.Write("Ocorreu um erro ao controlar a nave: " + ex.Message);
            }


            opt = Funcoes.GetOpt();
            Console.WriteLine("Opcao Selecionada: " + opt);
        }
    }
}