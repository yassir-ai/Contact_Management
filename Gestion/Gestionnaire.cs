using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Gestion
{
    [Serializable]
    public class Gestionnaire
    {
        public static Dossier racine;
        public static Dossier dossier_courant;

        public Gestionnaire()
        {
            racine = new Dossier("Root");
            dossier_courant = racine;
            racine.dossier_parent = racine;
        }

        public Contact recherche_contact(string nom, string prenom)
        {
            Dossier dos_courant = racine;
            Contact resultat = null;
            bool trouve = false;

            //Si il existe un seul dossier qui est la racine 
            if (Gestionnaire.dossier_courant == racine)
            {
                resultat = dos_courant.getListeContacte.Find(x => x.Nom == nom && x.Prenom == prenom);
            }
            else
            {
                while (dos_courant != Gestionnaire.dossier_courant && !trouve)
                {
                    resultat = dos_courant.getListeContacte.Find(x => x.Nom == nom && x.Prenom == prenom);
                    trouve = resultat != null;

                    if (!trouve)
                    {
                        dos_courant = dos_courant.gsDossier;
                    }
                }
            }

            return resultat;
        }

        public void supprimer_contact(string nom, string prenom)
        {
            Contact contact = recherche_contact(nom, prenom);

            if (contact != null)
            {
                contact.getDossierParent.getListeContacte.RemoveAt(contact.Position);
                Console.WriteLine("le contact a été supprimé");

                //date de modification du dossier parent ca etre change
                contact.getDossierParent.Date_modification = DateTime.Now;
            }
        }

        public void ajouter_contact(string nom, string prenom, string courriel, string societe, string lien)
        {
            Contact nouv_contact = new Contact(nom, prenom, courriel, societe, lien);
            dossier_courant.getListeContacte.Add(nouv_contact);
            nouv_contact.Position = dossier_courant.NombreContact++;
            Console.WriteLine("Contact " + prenom + " ajouté sous dossier en position 1");
            nouv_contact.getDossierParent.Date_modification = DateTime.Now;
        }


        public void ajouter_dossier(string nom)
        {
            Dossier nouv_dossier = new Dossier(nom);
            nouv_dossier.dossier_parent = dossier_courant;
            dossier_courant.gsDossier = nouv_dossier;
            dossier_courant = nouv_dossier;
            nouv_dossier.dossier_parent.Date_modification = DateTime.Now;
            Console.WriteLine("Dossier '" + nom + "' ajouté sous dossier en position 1");
        }

        public Dossier Recherche_prec_dossier(string nom)
        {
            Dossier dos_prec = racine;

            while (dos_prec.gsDossier != null && dos_prec.gsDossier.Nom != nom)
            {
                dos_prec = dos_prec.gsDossier;
            }

            return dos_prec;
        }

        public void supprimer_dossier(string nom)
        {
            Dossier prec_cible = Recherche_prec_dossier(nom);
            prec_cible.gsDossier = null;
            Gestionnaire.dossier_courant = prec_cible;
            prec_cible.Date_modification = DateTime.Now;
        }


        public void afficher_gestionnair()
        {
            Dossier cour_dossier = racine;
            int cmpt = 0;

            while (cour_dossier != null)
            {
                for (int i = 0; i < cmpt; i++) Console.Write(" ");
                cour_dossier.afficher_dossier(cmpt);
                cour_dossier = cour_dossier.gsDossier;
                cmpt++;
            }
        }
    }
}
