using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Gestion;
using Serialisation;

namespace ConsoleApp
{
    public class Program
    {
        public static SerialisationBinaire objetDeSerialisation = new SerialisationBinaire();
        public static SerialisationXML objetDeSerialisation1 = new SerialisationXML();
        public static string key = "";

        public static void enregistrer(string chemin_fichier)
        {
            Console.WriteLine("Entrez votre clé de cryptage de longueur 8 (laissez vide pour utiliser votre SID): ");
            key = Console.ReadLine();

            Console.WriteLine("Enregistrement du fichier '{0}' ...", chemin_fichier);
            objetDeSerialisation.serialisation(chemin_fichier, Gestionnaire.racine,ref key);
        }

        public static Dossier charger(ref string chemin_fichier) 
        {
            /*VARIABLES LOCALES*/
            string[] fichiers_trouves = null;                                                                         // Fichiers de DB trouvés dans le dossier 'Documents'
            string chemin_mes_documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);            // Variable qui récupère l'emplacement du dossier 'Documents' de l'utilisateur courant
            fichiers_trouves = Directory.GetFiles(chemin_mes_documents, "*.db");                                       // Cherche les fichiers .db trouvé dans 'Documents'
            string mon_fichier = "";
            Dossier result = new Dossier("result");

            if (fichiers_trouves.Length == 0)                                                                         // Si aucun fichier trouvé alors on créé un, sinon on charge le premier qu'on a trouvé
            {
                Console.WriteLine("Aucun fichier de DB trouvé  ");
                chemin_fichier = creationFichier();
            }
            else
            {
                Console.WriteLine("Voici vos fichiers qui sont déjà enregistrés :\n");
                Console.ForegroundColor = ConsoleColor.Yellow;   
                foreach (string e in fichiers_trouves)
                {
                    Console.WriteLine(e);
                }
                Console.ResetColor();
                Console.WriteLine("\nDonner le nom du fichier que vous voulez charger : (sans mentionner le .db)");
                mon_fichier = Console.ReadLine();
                chemin_fichier = Path.Combine(chemin_mes_documents, mon_fichier + ".db");

                try
                {
                    Console.WriteLine("Entrez votre clé de cryptage (laissez vide pour utiliser votre SID): ");

                     key = Console.ReadLine();
                    Gestionnaire.racine = objetDeSerialisation.deserialisation(chemin_fichier,ref key);                           //la racine prend le retour de la deserialisation
                    Console.WriteLine("\nChargement du fichier '{0}' ...", chemin_fichier);
                    Gestionnaire.dossier_courant = dernier_dossier(Gestionnaire.racine);                                                        // On récupère le dernier dossier créé, comme ça le dossier_courant pointe vers lui

                    Console.Write("Le fichier ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write(chemin_fichier);
                    Console.ResetColor();
                    Console.WriteLine(" où vos données sont enregistrées a été trouvé et il sera chargé.");
                    Console.WriteLine("N'oubliez pas d'enregistrer vos modifications !\n");
            }
                catch (Exception ex) { throw new Exception(); }

        }

            return Gestionnaire.dossier_courant;
        }


        /*Methode qui crée un fichier*/ 
        public static string creationFichier()
        {
            string nom_fichier = "";
            string chemin_mes_documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);            //recuperer le chemin vers 'mes documents'
            string chemin_fichier = "";

            Console.WriteLine("Veuillez renseigner le nom du fichier à créer (vide pour id windows): ");

            nom_fichier = Console.ReadLine();

            if (nom_fichier == "")                                                                               // Si vide alors on utilise l'id windows comme nom
            {
                nom_fichier = Environment.UserName;
            }

            chemin_fichier = Path.Combine(chemin_mes_documents, nom_fichier + ".db");                      // Combiner le chemin du dossier 'Documents' avec le nom du fichier et son extension
            Gestionnaire.dossier_courant = Gestionnaire.racine;                                            // envoyer dle dossier courant depuis le debut  
            Gestionnaire.racine.gsDossier = null;

            Console.Write("Création du fichier ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine( chemin_fichier + " ... ");
            Console.ResetColor();
            Console.WriteLine("N'oubliez pas d'enregistrer vos modifications !");

         
            return chemin_fichier;                                                              // On retourne filePath pour la stocker dans la variable correspondante dans Main
        }

        /*Chercher le dernier dossier créé dans une arborescence*/
        public static Dossier dernier_dossier(Dossier root)
        {
            Dossier cour = root;
            while (cour.gsDossier != null)
            {
                cour = cour.gsDossier;
            }
            return cour;
        }




        public static void Main(string[] args)
        {
            Gestionnaire gestionnair = new Gestionnaire();
            string chemin_fichier = "bonjour";
            bool ok = false;
            int i = 0;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Bonjour, vous voulez charger ou creer un fichier '.db' ?");
            Console.WriteLine(" 0- Charger");
            Console.WriteLine(" 1- Creer");
            Console.ResetColor();
            string choix = "";
            choix = Console.ReadLine();
            switch(choix)
            {
                case "0":
                    while (i < 3)
                    {
                        try
                        {
                            charger(ref chemin_fichier);
                            i = 3;
                            ok = true;
                        }
                        catch (FileNotFoundException p)
                        {
                            Console.Write("\nCe fichier n'existe pas, ça vous reste ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write((2 - i) + " tentative(s) !");
                            Console.ResetColor();
                            Console.WriteLine(" Réssayer !\n");
                            i++;
                        }
                        catch (SerializationException m)
                        {
                            Console.WriteLine("La clé utilisée n'est pas la bonne, réssayez plus tard ");
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.Write("\nPeut etre que ce fichier n'existe pas, ça vous reste ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write((2 - i) + " tentative(s) !");
                            Console.ResetColor();
                            i++;
                            key = "";
                            chemin_fichier = "";
                        }

                    }
                    break;

                case "1":
                    chemin_fichier = creationFichier();
                    ok = true;
                    break;

                default:
                    break;

            }





            if (ok)
            {
                string[] input;
                bool stop = false;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nBonjour ! Veuillez taper l'action que vous voulez");
                Console.WriteLine("Les actions possibles :");
                Console.WriteLine("1- sortir");
                Console.WriteLine("2- afficher");
                Console.WriteLine("3- charger ");
                Console.WriteLine("4- enregistrer");
                Console.WriteLine("5- ajouterdossier [nom du dossier]");
                Console.WriteLine("6- ajoutercontact [prenom] [nom] [societe] [courriel] [lien]\n");
                Console.ResetColor();

                while (!stop)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(">");
                    input = Console.ReadLine().Split(' ');
                    Console.ResetColor();

                    try
                    {
                        switch (input[0])
                        {
                            case "sortir":
                                stop = true;
                                break;

                            case "afficher":
                                gestionnair.afficher_gestionnair();
                                break;

                            case "charger":
                                Console.ForegroundColor= ConsoleColor.Red;
                                Console.WriteLine("Vous voulez enregistrer les modifications ? 1/ oui, 2/ non. Tapez 1 ou 2\n (NB : Si vous choisissez oui vous devez saisir  une nouvelle cle)");
                                choix = Console.ReadLine();
                                Console.ResetColor();

                                if (int.Parse(choix) == 1)
                                {
                                    enregistrer(chemin_fichier);
                                    Gestionnaire.dossier_courant = charger(ref chemin_fichier);
                                }
                                else if (int.Parse(choix) == 2) Gestionnaire.dossier_courant = charger(ref chemin_fichier);
                                break;

                            case "enregistrer":
                                Console.WriteLine("NB: Apperement vous avez modifier le fichier, donc veuillez saisir maintenant une nouvelle cle");
                                enregistrer(chemin_fichier);
                                break;

                            case "ajouterdossier":
                                gestionnair.ajouter_dossier(input[1]);
                                break;

                            case "ajoutercontact":
                                gestionnair.ajouter_contact(input[2], input[1], input[4], input[3], input[5]);
                                break;

                            default:
                                Console.WriteLine("Instruction Iconnue.");
                                break;
                        }
                    }
                     catch (FileNotFoundException y)
                    {
                         Console.WriteLine("Désolé, ce fichier n'existe pas !");
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Console.WriteLine("Parametres de saisis insuffisants");
                    }
                    catch (SerializationException e)
                    {
                        Console.WriteLine("la clé utilisé n'est pas la bonne, réssayez plus tard");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Une erreur est survenue");
                    }
                }
            }
        }
    }
}
