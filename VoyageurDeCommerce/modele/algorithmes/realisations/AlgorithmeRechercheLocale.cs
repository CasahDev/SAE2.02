using System;
using System.Collections.Generic;
using VoyageurDeCommerce.modele.distances;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    /// <summary>
    /// Exemple de classe Algorithme, à ne pas garder
    /// </summary>
    public class AlgorithmeRechercheLocale : Algorithme
    {

        public override string Nom => "Recherche locale";
        
        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            FloydWarshall.calculerDistances(listeLieux,listeRoute);
            Algorithme algo = new AlgorithmePlusProcheVoisin();
            algo.Executer(listeLieux,listeRoute);
            this.Tournee = algo.Tournee;
            Console.WriteLine($"Tournee : {Tournee} distance : {Tournee.Distance}");

            
            ChercherVoisine();
            Console.WriteLine("Ok fin executer");
            Console.WriteLine($"Tournee : {Tournee} distance : {Tournee.Distance}");

        }

        private void ChercherVoisine()
        {
            Console.WriteLine("Ok début cherchervoisine");

            int distance = Tournee.Distance;
            List<Lieu> liste = Tournee.ListeLieux;
            
            for (int i = 0; i < Tournee.ListeLieux.Count; i++)
            {
                Tournee.ListeLieux = TourneeMin(liste, distance, i);
            }
            Console.WriteLine("Ok fin cherchervoisin");

        }

        private int CalculeDistance(List<Lieu> listeLieux, int distance, int index)
        {
            Console.WriteLine("Ok début calculdistance");

            int result = 0;
            

            if (index+1 >= listeLieux.Count)
            {
                result = distance + FloydWarshall.Distance(listeLieux[index], listeLieux[index - 1]) +
                         FloydWarshall.Distance(listeLieux[0], listeLieux[1]) -
                         FloydWarshall.Distance(listeLieux[index - 1], listeLieux[0]) -
                         FloydWarshall.Distance(listeLieux[index], listeLieux[0]);
            }
            else if (index+2>=listeLieux.Count)
            {
                result = distance + FloydWarshall.Distance(listeLieux[index], listeLieux[index - 1]) +
                         FloydWarshall.Distance(listeLieux[index + 1], listeLieux[0]) -
                         FloydWarshall.Distance(listeLieux[index - 1], listeLieux[index + 1]) -
                         FloydWarshall.Distance(listeLieux[index], listeLieux[index + 1]);
            }
            else if (index == 0)
            {
                result = distance + FloydWarshall.Distance(listeLieux[index], listeLieux[listeLieux.Count-1]) +
                         FloydWarshall.Distance(listeLieux[index + 1], listeLieux[index + 2]) -
                         FloydWarshall.Distance(listeLieux[listeLieux.Count-1], listeLieux[index + 1]) -
                         FloydWarshall.Distance(listeLieux[index], listeLieux[index + 1]);
            }
            else
            {
                result = distance + FloydWarshall.Distance(listeLieux[index], listeLieux[index - 1]) +
                         FloydWarshall.Distance(listeLieux[index + 1], listeLieux[index + 2]) -
                         FloydWarshall.Distance(listeLieux[index - 1], listeLieux[index + 1]) -
                         FloydWarshall.Distance(listeLieux[index], listeLieux[index + 1]);
            }
            Console.WriteLine("Ok fin calculdistance");
            return result;
        }

        private List<Lieu> TourneeMin(List<Lieu> listeLieux, int distance, int index)
        {
            Console.WriteLine("Ok début tourneemin");

            int minimum = distance;
            List<Lieu> result = listeLieux;
            int nvDistance = CalculeDistance(listeLieux, distance, index);
            
            if (nvDistance < minimum)
            {
                minimum = nvDistance;
                result = Exchange(listeLieux, index);
            }

            Console.WriteLine("Ok fin tourneemin");

            return result;
        }

        private List<Lieu> Exchange(List<Lieu> listeLieux, int index)
        {
            Console.WriteLine("Ok début exchange");

            Lieu temp;
            if (index == 0)
            {
                temp = listeLieux[index];
                listeLieux[index] = listeLieux[listeLieux.Count - 1];
                listeLieux[listeLieux.Count - 1] = temp;
            }
            else
            {
                temp = listeLieux[index];
                listeLieux[index] = listeLieux[index - 1];
                listeLieux[index - 1] = temp;
            }
            Console.WriteLine("Ok fin exchange");

            return listeLieux;
        }
        
    }
}