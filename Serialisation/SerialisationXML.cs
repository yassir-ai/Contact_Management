using Gestion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Serialisation
{
    public class SerialisationXML
    {
        public void Serialization(string filePath, Dossier racine)
        {
            XmlSerializer b = new XmlSerializer(typeof(Dossier), new Type[] { typeof(Dossier), typeof(Contact) });
            FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();
            string key = "";
            CryptoStream crStream = null;

            Console.WriteLine("Entrez votre clé de cryptage de longueur 8 (laissez vide pour utiliser votre SID): ");
            key = Console.ReadLine();
            if (key == "")                                               // Si clé vide donc on utilise le SID
            {
                key = WindowsIdentity.GetCurrent().User.ToString();
            }

            try
            {
                desCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                desCryptoServiceProvider.IV = Encoding.ASCII.GetBytes("ABCDEFGH");

                crStream = new CryptoStream(file, desCryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
                b.Serialize(crStream, racine);
                Console.WriteLine("Opération réussie\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Erreur lors de la sérialisation du fichier\n");
            }
            if (crStream != null) crStream.Close();
            if(file != null) file.Close();
        }
        public Dossier Deserialization(string filePath)
        {
            Dossier root = new Dossier();
            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
            XmlSerializer b = new XmlSerializer(typeof(Dossier), new Type[] { typeof(Dossier), typeof(Contact) });
            DESCryptoServiceProvider desCryptoServiceProvider = new DESCryptoServiceProvider();
            string key = "";
            CryptoStream crStream = null;

            Console.WriteLine("Entrez votre clé de cryptage de longueur 8 (laissez vide pour utiliser votre SID): ");
            key = Console.ReadLine();
            if (key == "")                                                  // Si clé vide donc on utilise le SID
            {
                key = WindowsIdentity.GetCurrent().User.ToString();
            }

            try
            {
                desCryptoServiceProvider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 8));
                desCryptoServiceProvider.IV = Encoding.ASCII.GetBytes("ABCDEFGH");

                crStream = new CryptoStream(file, desCryptoServiceProvider.CreateDecryptor(desCryptoServiceProvider.Key, desCryptoServiceProvider.IV), CryptoStreamMode.Read);
                root = (Dossier) b.Deserialize(crStream);
                Console.WriteLine("Fichier '{0}' chargé\n", filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Erreur lors de la désérialisation du fichier, une nouvelle structure hiérarchique sera créée\n");
            }
            if (crStream != null) crStream.Close();
            if (file != null) file.Close();

            return root;
        }

    }
}
