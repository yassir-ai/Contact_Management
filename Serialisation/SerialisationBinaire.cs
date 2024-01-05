using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using Gestion;
using System.IO;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Serialisation
{
    public class SerialisationBinaire
    {
        public void serialisation(string filePath, Dossier racine,ref string key) 
        {
            BinaryFormatter a = new BinaryFormatter();
            FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();                      //pour effectuer le chiffrement
            CryptoStream crStream = null;                                                                            //pour crypter les données

            if (key == "")                                                                                           // Si clé vide donc on utilise le SID
            {
                key = WindowsIdentity.GetCurrent().User.ToString();
            }

            try
            {
                desCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                desCryptoServiceProvider.IV = Encoding.ASCII.GetBytes("ABCDEFGH");
                crStream = new CryptoStream(file, desCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
                a.Serialize(crStream, racine);
                Console.WriteLine("Opération réussie !");
            }
            catch (FileNotFoundException f) { throw new FileNotFoundException(); }
            catch(SerializationException p) { throw new SerializationException(); }
            catch (Exception e) { Console.WriteLine("Réssayez et donnez une bonne cle"); }
        
            if (crStream != null) crStream.Close();
            if(file != null) file.Close();
        }



        public Dossier deserialisation(string filePath,ref string key)
        {
            Dossier racine = new Dossier();
            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None); ;
            BinaryFormatter b = new BinaryFormatter();
            DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();
            CryptoStream crStream = null;

            if (key == ""  )                                      // Si clé vide donc on utilise le SID
            {
                key = WindowsIdentity.GetCurrent().User.ToString();
            }

            try
            {
                desCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                desCryptoServiceProvider.IV = Encoding.ASCII.GetBytes("ABCDEFGH");

                crStream = new CryptoStream(file, desCryptoServiceProvider.CreateDecryptor(desCryptoServiceProvider.Key, desCryptoServiceProvider.IV), CryptoStreamMode.Read);

                racine = (Dossier) b.Deserialize(crStream);
                Console.WriteLine("Opération réussie !");
            }
            catch (Exception e)
            {
                throw new Exception();                             //lever l exception au main
            }

            if (crStream != null)  crStream.Close();
            if (file != null) file.Close();

            return racine;
        }
    }
}
