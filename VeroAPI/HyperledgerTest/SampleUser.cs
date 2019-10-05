using System;
using System.Collections.Generic;
using System.IO;
using Hyperledger.Fabric.SDK;
using Hyperledger.Fabric.SDK.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HyperledgerTest
{
    public class SampleUser : IUser
    {
        //private readonly Hyperledger.Fabric.SDK.Security.KeyStore keyValStore;
        private string account;

        private string affiliation;

        private IEnrollment enrollment;

        private string enrollmentSecret;

        /**
         * Save the state of this user to the key value store.
         */
        private bool loading;

        private string mspId;
        private HashSet<string> roles;

        private ICryptoSuite cryptoSuite;
        public static SampleUser Load(string name, string store_path)
        {
            var content = File.ReadAllText(Path.Combine(store_path, name));
            var sampleUser = Newtonsoft.Json.JsonConvert.DeserializeObject<SampleUser>(content);
            ((Enrollment)sampleUser.Enrollment).signingIdentity = Path.Combine(store_path, ((Enrollment)sampleUser.Enrollment).signingIdentity)+"-priv";
            return sampleUser;
        }
        public SampleUser(string name, string org, /*SampleStore fs,*/ ICryptoSuite cryptoSuite)
        {
            this.Name = name;
            this.cryptoSuite = cryptoSuite;

            //keyValStore = fs;
            //Organization = org;
            KeyValStoreName = ToKeyValStoreName(Name, org);
            //string memberStr = keyValStore.GetValue(KeyValStoreName);
            //if (null == memberStr)
            //{
            //    SaveState();
            //}
            //else
            //{
            //    RestoreState();
            //}
        }

        public SampleUser(string name, string mspid, Enrollment enrollment)
        {
            this.Name = name;
            this.Enrollment = enrollment;
            this.MspId = mspid;
        }
        
        [JsonConstructor]
        public SampleUser(string name,string account, string affiliation, IEnrollment enrollment, string enrollmentSecret, bool loading, string mspId, HashSet<string> roles)
        {
            this.Name = name;
            this.account = account;
            this.affiliation = affiliation;
            this.enrollment = enrollment;
            this.enrollmentSecret = enrollmentSecret;
            this.loading = loading;
            this.mspId = mspId;
            this.roles = roles;
        }

        public string Organization { get; set; }


        public string EnrollmentSecret
        {
            get => enrollmentSecret;
            set
            {
                enrollmentSecret = value;
                SaveState();
            }
        }


        public string KeyValStoreName { get; set; }


        /* Determine if this name has been registered.
        * * @return {
            @code true
        } if registered;

        otherwise {
            @code false
        }.*/

        public bool IsRegistered => !string.IsNullOrEmpty(EnrollmentSecret);

        /**
         * Determine if this name has been enrolled.
         *
         * @return {@code true} if enrolled; otherwise {@code false}.
         */
        public bool IsEnrolled => enrollment != null;


        public string Name { get; }


        public HashSet<string> Roles
        {
            get => roles;
            set
            {
                roles = value;
                SaveState();
            }
        }


        public string Account
        {
            get => account;
            set
            {
                account = value;
                SaveState();
            }
        }


        public string Affiliation
        {
            get => affiliation;
            set
            {
                affiliation = value;
                SaveState();
            }
        }

        [JsonConverter(typeof(InterfaceConverter<IEnrollment, Enrollment>))]
        public IEnrollment Enrollment
        {
            get => enrollment;
            set
            {
                enrollment = value;
                SaveState();
            }
        }

        public string MspId
        {
            get => mspId;
            set
            {
                mspId = value;
                SaveState();
            }
        }

        HashSet<string> IUser.Roles => throw new NotImplementedException();

        //public static bool IsStored(string name, string org, SampleStore fs)
        //{
        //    return fs.HasValue(ToKeyValStoreName(name, org));
        //}

        public void SaveState()
        {
            if (!loading)
            {
                string str = JsonConvert.SerializeObject(this);
                //keyValStore.SetValue(KeyValStoreName, str.ToBytes().ToHexString());
            }
        }

        /**
         * Restore the state of this user from the key value store (if found).  If not found, do nothing.
         */
        public SampleUser RestoreState()
        {
            //loading = true;
            //try
            //{
            //    string memberStr = keyValStore.GetValue(KeyValStoreName);
            //    if (null != memberStr)
            //    {
            //        JsonConvert.PopulateObject(memberStr.FromHexString().ToUTF8String(), this);
            //        return this;
            //    }
            //}
            //catch (System.Exception e)
            //{
            //    throw new System.Exception($"Could not restore state of member {Name}", e);
            //}
            //finally
            //{
            //    loading = false;
            //}

            return null;
        }


        public static string ToKeyValStoreName(string name, string org)
        {
            return "user." + name + org;
        }
    }

    public class InterfaceConverter<TInterface, TConcrete> : CustomCreationConverter<TInterface> where TConcrete : TInterface, new()
    {
        public override TInterface Create(Type objectType)
        {
            return new TConcrete();
        }
    }


    public class Enrollment : IEnrollment
    {
        public string signingIdentity { get; set; }
        public Identity identity { get; set; }

        public string Key => File.Exists(signingIdentity)?File.ReadAllText(signingIdentity):"";

        public string Cert => identity.certificate;
    }

    public class Identity
    {
        public string certificate { get; set; }
    }

}
