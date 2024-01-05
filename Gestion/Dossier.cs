using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion
{
    [Serializable]
    public class Dossier
    {
        private string nom;
        private DateTime date_creation;
        private DateTime date_modification;
        private Dossier dossier;                    //le dossier contenu dans this
        public Dossier dossier_parent;                    //le dossier parent de this
        private List<Contact> liste_contact;
        private int nombre_contact;

        public Dossier(string nom)
        {
            this.nom = nom;
            liste_contact = new List<Contact>();
            date_modification = DateTime.Now;
            date_creation = DateTime.Now;
            nombre_contact = 0;
            dossier = null;
        }

        public int NombreContact
        {
            get => nombre_contact;
            set => nombre_contact = value;
        }

        public Dossier()
        { }

        public string Nom
        {
            get => nom;
            set
            {
                nom = value;
                date_modification = DateTime.Now;
            }
        }

        public List<Contact> getListeContacte
        {
            get => liste_contact;
        }

        public Dossier gsDossier
        {
            get => dossier;
            set => dossier = value;
        }

        public DateTime Date_creation
        {
            get => date_creation;
        }
        public DateTime Date_modification 
        {
            get => date_modification;
            set => date_modification = value;
        }

        public void ajout_dossier(string nom)
        {
            dossier = new Dossier(nom);
        }

        public void afficher_dossier(int cmpt)
        {
            Console.WriteLine("[D] " + nom + " (création " + date_creation + ")");
            foreach (Contact contact in liste_contact)
            {
                for (int i = 0; i < cmpt; i++) Console.Write(" ");
                contact.afficher_contact();
            }
        }


    }
}
