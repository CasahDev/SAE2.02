using System.Collections.Generic;
using System.Linq;
using VoyageurDeCommerce.modele.distances;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    public class AlgorithmeInsertionLoin : Algorithme
    {
        private List<Lieu> nonVisites;
        
        public override string Nom => "Insertion loin";

        /// <summary>
        /// Définit l'exécution de l'algorithme
        /// </summary>
        /// <param name="listeLieux">Les différents lieux du graphe</param>
        /// <param name="listeRoute">Les différentes routes du graphe</param>
        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            FloydWarshall.calculerDistances(listeLieux,listeRoute);
            
            this.nonVisites = listeLieux;
            GetFarthestCouple(listeLieux);
            
        }

        private void GetFarthestCouple(List<Lieu> listeLieux)
        {
            int distanceMax = 0;
            Lieu depart = null;
            Lieu arrivee = null;
            for (int i = 0; i < listeLieux.Count-1; i++)
            {
                for (int j = i+1; j < listeLieux.Count ; j++)
                {
                    
                    if (FloydWarshall.Distance(listeLieux[i], listeLieux[j])>distanceMax)
                    {
                        distanceMax = FloydWarshall.Distance(listeLieux[i], listeLieux[j]);
                        depart = listeLieux[i];
                        arrivee = listeLieux[j];
                    }
                }
            }
            Tournee.Add(depart);
            Tournee.Add(arrivee);
            this.nonVisites.Remove(depart);
            this.nonVisites.Remove(arrivee);
        }
        
        private int DistanceFromTournee(List<Lieu> listeLieux,Lieu sommet)
        {
            List<int> distances = new List<int>();
            List<Lieu> couple = new List<Lieu>();
            for (int i = 0; i < listeLieux.Count-2; i++)
            {
                couple[0] = listeLieux[i];
                couple[1] = listeLieux[i + 1];
                distances.Add(DistanceFromCouple(sommet,couple));
            }

            couple[0] = listeLieux[listeLieux.Count - 1];
            couple[1] = listeLieux[0];
            distances.Add(DistanceFromCouple(sommet,couple));
            return distances.Min();
        }

        private int DistanceFromCouple(Lieu sommet, List<Lieu> couple)
        {
            return FloydWarshall.Distance(sommet, couple[0]) + FloydWarshall.Distance(sommet, couple[1]) -
                   FloydWarshall.Distance(couple[0], couple[1]);
        }

        private Lieu FarthestFromTournee(List<Lieu> listeLieux)
        {
            Lieu result = null;
            int max = 0;
            while (this.nonVisites != null)
            {
                foreach (Lieu lieu in listeLieux)
                {
                    if (DistanceFromTournee(listeLieux,lieu)>max)
                    {
                        max = DistanceFromTournee(listeLieux,lieu);
                        result = lieu;
                    }

                    this.nonVisites.Remove(lieu);
                }
            }

            return result;
        }
    }
}