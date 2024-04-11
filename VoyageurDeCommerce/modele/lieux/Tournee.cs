using System.Collections.Generic;
using VoyageurDeCommerce.modele.distances;
using VoyageurDeCommerce.vuemodele;

namespace VoyageurDeCommerce.modele.lieux
{
    ///<summary>Tour (ensemble ordonné de lieu parcouru dans l'ordre avec retour au point de départ)</summary>
    public class Tournee
    {
        /// <summary>Liste des lieux dans l'ordre de visite</summary>
        private List<Lieu> listeLieux;
        public List<Lieu> ListeLieux
        {
            get => listeLieux;
            set => listeLieux = value;
        }

        /// <summary>Constructeur par défaut</summary>
        public Tournee()
        {
            this.listeLieux = new List<Lieu>();
        }

        /// <summary>Constructeur par copie</summary>
        public Tournee(Tournee modele)
        {
            this.listeLieux = new List<Lieu>(modele.listeLieux);
        }

        /// <summary>
        /// Ajoute un lieu à la tournée (fin)
        /// </summary>
        /// <param name="lieu">Le lieu à ajouter</param>
        public void Add(Lieu lieu)
        {
            this.ListeLieux.Add(lieu);
        }

        /// <summary>Distance totale de la tournée</summary>
        public int Distance
        {
            get
            {
                int d = 0;
                for (int i = 1; i < listeLieux.Count; i++)
                {
                    d += FloydWarshall.Distance(listeLieux[i - 1], ListeLieux[i]);
                }

                d += FloydWarshall.Distance(listeLieux[listeLieux.Count - 1], listeLieux[0]);
                return d;
            }
        }

        public override string ToString()
        {
            string message = "";
            foreach (var lieu in listeLieux)
            {
                message += lieu.Nom + "=>";
            }

            message += listeLieux[0].Nom;
            return message;
        }
    }
}
