using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Hyperledger.Fabric.SDK;
using Hyperledger.Fabric.SDK.Channels;
using Hyperledger.Fabric_CA.SDK;

namespace VeroAPI
{
    public class UserManager
    {
        private string keyStorePath;
        private SampleUser admin;
        private HFClient fabric_client;
        private Channel channel;
        private HFCAClient fabric_ca_client;

        public UserManager()
        {
            keyStorePath = @"hfc-key-store/";
            admin = SampleUser.Load("admin", keyStorePath);
            fabric_client = HFClient.Create();
            var crypto = new Hyperledger.Fabric.SDK.Security.CryptoPrimitives();
            crypto.Init();
            crypto.Store.AddCertificate(admin.Enrollment.Cert);

            fabric_client.CryptoSuite = crypto;

            fabric_client.UserContext = admin;

            channel = fabric_client.NewChannel("mychannel");
            var peer = fabric_client.NewPeer("p1", $"grpc://{Startup.HyperleaderServer}:7051");
            channel.AddPeer(peer);

            var ordered = fabric_client.NewOrderer("o1", $"grpc://{Startup.HyperleaderServer}:7050");
            channel.AddOrderer(ordered);

            channel.Initialize();

            fabric_ca_client = new Hyperledger.Fabric_CA.SDK.HFCAClient("", $"http://{Startup.HyperleaderServer}:7054", new Hyperledger.Fabric.SDK.Helper.Properties());
            fabric_ca_client.CryptoSuite = crypto;

        }
        public void Listar()
        {
            try
            {
                var list = fabric_ca_client.GetHFCAIdentities(admin);
                foreach (var item in list)
                {

                    Console.WriteLine($"affiliation: {item.Affiliation}");
                    Console.WriteLine($"EnrollmentId: {item.EnrollmentId}");
                    Console.WriteLine($"IsDeleted: {item.IsDeleted}");
                    Console.WriteLine($"Secret: {item.Secret}");
                    Console.WriteLine($"Type: {item.Type}");
                    Console.WriteLine("----------------------------------------------------------");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        public void Remover(string userId)
        {
            try
            {
                var list = fabric_ca_client.GetHFCAIdentities(admin);

                var user = list.FirstOrDefault(x => x.EnrollmentId == userId);
                if (user != null)
                {
                    var test = fabric_ca_client.NewHFCAIdentity(userId);
                    var resp = test.Delete(admin);
                    if (resp == 1)
                    {
                        if (File.Exists(Path.Combine(keyStorePath, userId)))
                        {
                            var file = SampleUser.Load(userId, keyStorePath);
                            File.Delete(Path.Combine(keyStorePath, userId));
                            File.Delete(Path.Combine(keyStorePath, ((Enrollment)file.Enrollment).signingIdentity));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
        public void Register(string userName)
        {

            var secret = fabric_ca_client.Register(new Hyperledger.Fabric_CA.SDK.Requests.RegistrationRequest(userName, "org1.department1")
            {
                EnrollmentID = userName,
                Type = "client"
            }, admin);

            Console.WriteLine("Successfully registered user1 - secret:" + secret);

            try
            {
                var enrollment = fabric_ca_client.Enroll(userName, secret);

                Console.WriteLine($"Successfully enrolled member user '{userName}' ");
                string mspid = "Org1MSP";
                var newUser = new SampleUser(userName, mspid, new Enrollment() { identity = new Identity() { certificate = enrollment.Cert }, signingIdentity = secret });

                string enrollmentFile = Path.Combine(keyStorePath, userName);
                string pkeyFile = Path.Combine(keyStorePath, secret + "-priv");
                var content = Newtonsoft.Json.JsonConvert.SerializeObject(newUser);
                File.WriteAllText(enrollmentFile, content);
                File.WriteAllText(pkeyFile, enrollment.Key);

                var userFolder = Path.Combine(keyStorePath, userName + "_temp");
                if (Directory.Exists(userFolder))
                {
                    Directory.Delete(userFolder, true);
                }

                Directory.CreateDirectory(userFolder);

                File.Copy(enrollmentFile, Path.Combine(userFolder, userName));
                File.Copy(pkeyFile, Path.Combine(userFolder, secret + "-priv"));

                ZipFile.CreateFromDirectory(userFolder, userFolder.Replace("_temp", ".zip"));
            }
            catch (Exception ex)
            {

            }
        }

        public FileStream GetUserCredentials(string userName)
        {
            var zip = Path.Combine(keyStorePath, userName + ".zip");
            if (File.Exists(zip))
                return File.OpenRead(zip);

            return null;
        }
    }
}
