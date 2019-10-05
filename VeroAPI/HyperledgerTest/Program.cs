using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Hyperledger.Fabric.Protos.Msp.MspConfig;
using Hyperledger.Fabric.SDK.AMCL;
using Hyperledger.Fabric.SDK.AMCL.FP256BN;
using Hyperledger.Fabric.SDK.Idemix;

namespace HyperledgerTest
{
    class Program
    {
        public static string ServerIP = "10.115.65.33";
        static void Main(string[] args)
        {
            while (!Console.TreatControlCAsInput)
            {
                Console.WriteLine("1 - Gerenciar usuário");
                Console.WriteLine("2 - Gerenciar Fabrica de autos");
                Console.WriteLine("3 - Gerenciar Vero");
                Console.WriteLine();
                Console.WriteLine("0 - Sair");
                Console.WriteLine("-----------------------------------------------");
                Console.Write("Resposta: ");
                var read = Console.ReadLine();
                Console.WriteLine("-----------------------------------------------");
                int response;
                if (int.TryParse(read, out response))
                {
                    switch (response)
                    {
                        case 1:
                            UserManagerMenu();
                            break;
                        case 2:
                            FabcarMenu();
                            break;
                        case 3:
                            VeroMenu();
                            break;
                        case 0:
                            Console.TreatControlCAsInput = true;
                            break;
                        default:
                            break;
                    }

                }
                Console.Clear();
            }
        }

        private static void VeroMenu()
        {
            Console.Write("Usuario: ");
            var user = Console.ReadLine();

            VeroChain vero = new VeroChain(user);
            bool sair = false;
            while (!sair)
            {
                Console.Clear();
                Console.WriteLine("1 - Salvar Info");
                Console.WriteLine("2 - Buscar Info");
                Console.WriteLine("3 - Apagar Info");
                Console.WriteLine();
                Console.WriteLine("0 - Sair");
                Console.WriteLine("-----------------------------------------------");
                Console.Write("Resposta: ");
                var read = Console.ReadLine();
                Console.WriteLine("-----------------------------------------------");
                int response;
                if (int.TryParse(read, out response))
                {
                    switch (response)
                    {
                        case 1:
                            var info = @"
==============================================================
+                                                            +
+   Primeiro entre com o id e em seguida entre com as        +
+   chaves e os valores, ao finalizar deixe a chave em       +
+   branco                                                   +
+                                                            +
==============================================================";
                            Console.WriteLine(info);
                            Console.WriteLine();
                            Dictionary<string, string> dic = new Dictionary<string, string>();
                            var suggest = Guid.NewGuid().ToString();
                            Console.Write($"ID[{suggest}]: ");
                            var id = Console.ReadLine();
                            id = string.IsNullOrEmpty(id) ? suggest : id;
                            string key = "";
                            do
                            {
                                Console.Write("Key: ");
                                key = Console.ReadLine();
                                if (!string.IsNullOrEmpty(key))
                                {
                                    Console.Write("Value: ");
                                    dic[key] = Console.ReadLine();
                                }
                                Console.WriteLine();
                            } while (!string.IsNullOrEmpty(key));
                            vero.SalvarInfo(id, dic);

                            break;
                        case 2:
                            Console.Write("ID: ");
                            id = Console.ReadLine();
                            var result = vero.BuscarInfo(id);
                            if (result != null)
                            {
                                Console.Clear();
                                foreach (var item in result)
                                {
                                    Console.WriteLine($"{item.Key}: {item.Value}");
                                }
                                Console.Read();
                            }
                            break;
                        case 3:
                            Console.Write("ID: ");
                            id = Console.ReadLine();
                            vero.InutilizarRegistro(id);
                           
                            break;
                        case 0:
                            sair = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public static void UserManagerMenu()
        {
            UserManager user = new UserManager();
            bool sair = false;
            while (!sair)
            {
                Console.WriteLine("1 - Criar um usuário");
                Console.WriteLine("2 - Listar  usuários");
                //Console.WriteLine("3 - Excluir usuário");
                Console.WriteLine("-----------------------------------------------");
                Console.Write("Resposta: ");
                var read = Console.ReadLine();
                Console.WriteLine("-----------------------------------------------");
                int response;
                if (int.TryParse(read, out response))
                {
                    switch (response)
                    {
                        case 1:
                            Console.Write("Nome: ");

                            user.Register(Console.ReadLine());
                            break;
                        case 2:
                            user.Listar();
                            break;
                        case 3:
                            Console.Write("Nome: ");
                            user.Remover(Console.ReadLine());
                            break;
                        case 0:
                            sair = true;
                            break;
                        default:
                            break;
                    }
                    if (!sair)
                    {
                        Console.WriteLine();
                        Console.Write("Pressione qualquer tecla para continuar...");
                        Console.ReadKey();
                    }

                }
                Console.Clear();
            }
        }
        public static void FabcarMenu()
        {
            try
            {
                Console.Write("Usuario: ");
                var user = Console.ReadLine();
                FabCar fab = new FabCar(user);
                bool sair = false;
                while (!sair)
                {
                    Console.WriteLine("1 - Listar todos os carros");
                    Console.WriteLine("2 - Criar um novo carro");
                    Console.WriteLine("3 - Consultar um carro");
                    Console.WriteLine("-----------------------------------------------");
                    Console.Write("Resposta: ");
                    var read = Console.ReadLine();
                    Console.WriteLine("-----------------------------------------------");
                    int response;
                    if (int.TryParse(read, out response))
                    {
                        switch (response)
                        {
                            case 1:
                                var result = fab.QueryAllCars();
                                foreach (var item in result)
                                {
                                    Console.WriteLine(item.ToString());
                                }
                                Console.WriteLine();
                                Console.Write("Pressione qualquer tecla para continuar...");
                                Console.ReadKey();
                                break;
                            case 2:
                                Console.Write("Key: ");
                                var key = Console.ReadLine();

                                Console.Write("color: ");
                                var color = Console.ReadLine();
                                Console.Write("make: ");
                                var make = Console.ReadLine();

                                Console.Write("model: ");
                                var model = Console.ReadLine();

                                Console.Write("owner: ");
                                var owner = Console.ReadLine();
                                fab.CreateCar(new FabCarItem()
                                {
                                    Key = key,
                                    Record = new Record()
                                    {
                                        color = color,
                                        make = make,
                                        model = model,
                                        owner = owner
                                    }
                                });
                                break;
                            case 3:
                                Console.Write("Key: ");
                                var key2 = Console.ReadLine();
                                var result2 = fab.QueryCar(key2);

                                Console.WriteLine(result2?.ToString());

                                Console.WriteLine();
                                Console.Write("Pressione qualquer tecla para continuar...");
                                Console.ReadKey();
                                break;
                            case 0:
                                sair = true;
                                break;
                            default:
                                break;
                        }

                    }
                    Console.Clear();
                }
            }
            catch (Exception e)
            {

            }
        }

    }

}
