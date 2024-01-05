using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Gestion
{
    [Serializable]
    public class Contact
    {
        private string nom;
        private string prenom;
        private string courriel;
        private string societe;
        private string lien;
        private DateTime date_creation;
        private DateTime date_modification;
        private Dossier dossier_parent;
        private int position;             //c'est sa position dans la liste des contact dans le dossier_parent

        public Contact(string nom, string prenom, string courriel, string societe, string lien)
        {

            try
            {
                MailAddress test = new MailAddress(courriel);
                this.courriel = courriel;
            }
            catch (FormatException e)
            {
                Console.WriteLine("Adresse email non valide! Veuillez réssayez après (Le champ mail reste pour le moment vide)");
            }
            finally
            {
                this.nom = nom;
                this.prenom = prenom;
                this.societe = societe;
                this.lien = lien;
                date_creation = DateTime.Now;
                date_modification = DateTime.Now;
                dossier_parent = Gestionnaire.dossier_courant;
            }
        }

        public Contact()
        { }

        public int Position
        {
            get => position;
            set => position = value;
        }

        public Dossier getDossierParent
        {
            get => dossier_parent;
        }

        public string Nom
        {
            get => nom;
            set
            {
                nom = value;
                date_modification = DateTime.Now;
            }
        }
        public string Prenom
        {
            get => prenom;
            set
            {
                prenom = value;
                date_modification = DateTime.Now;
            }
        }
        public string Courriel
        {
            get => courriel;
            set
            {
                courriel = value;
                date_modification = DateTime.Now;
            }
        }
        public string Societe
        {
            get => societe;
            set
            {
                societe = value;
                date_modification = DateTime.Now;
            }
        }
        public string Lien
        {
            get => lien;
            set
            {
                lien = value;
                date_modification = DateTime.Now;
            }
        }
        public DateTime Date_creation
        {
            get => date_creation;
        }
        public DateTime Date_modification
        {
            get => date_modification;
        }


        ~Contact()
        {
            
        }

        public void afficher_contact()
        {
            Console.WriteLine("| [C] " + nom + ", " + prenom + ", (" + societe + "), Email:" + courriel + ", Link:" + lien);
        }
    }
}
