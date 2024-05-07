using System.Collections.Generic;
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
            GetFarestCouple(listeLieux,listeRoute);
        }

        private void GetFarestCouple(List<Lieu> listeLieux, List<Route> listeRoute)
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
        }
    }
}