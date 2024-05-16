using System;
using System.Collections.Generic;
using System.Diagnostics;
using VoyageurDeCommerce.modele.distances;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    /// <summary>
    /// Exemple de classe Algorithme, à ne pas garder
    /// </summary>
    public class AlgorithmeRechercheLocale : Algorithme
    {
        private Stopwatch sw;

        public override string Nom => "Recherche locale";
        
        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            bool condition = false;
            int compteur = 0;
            
            sw = Stopwatch.StartNew();
            sw.Start();
            
            FloydWarshall.calculerDistances(listeLieux,listeRoute);
            Algorithme algo = new AlgorithmeCroissant();
            algo.Executer(listeLieux,listeRoute);


            foreach (Lieu lieu in algo.Tournee.ListeLieux)
            {
                this.Tournee.ListeLieux.Add(lieu);
                sw.Stop();
                NotifyPropertyChanged("Tournee");
                sw.Start();
            }

            while (condition == false && compteur < 10)
            {
                condition = ChercherVoisine();
                compteur++;
            }
            sw.Stop();
            TempsExecution = sw.ElapsedMilliseconds;

            
        }

        private bool ChercherVoisine()
        {

            List<Lieu> liste = Tournee.ListeLieux;
            Tournee tournee = new Tournee(Tournee);
            
            for (int i = 0; i < Tournee.ListeLieux.Count; i++)
            {
                TourneeMin(liste, i);
            }

            return tournee == Tournee;
        }

        private int CalculeDistance(List<Lieu> listeLieux, int distance, int index)
        {

            int result = 0;
            

            if ((index+1) == listeLieux.Count)
            {
                result = distance + FloydWarshall.Distance(listeLieux[0], listeLieux[index - 1]) +
                         FloydWarshall.Distance(listeLieux[index], listeLieux[1]) -
                         FloydWarshall.Distance(listeLieux[index], listeLieux[0]) -
                         FloydWarshall.Distance(listeLieux[1], listeLieux[index-1]);
            }
            else if ((index+2) == listeLieux.Count)
            {
                result = distance + FloydWarshall.Distance(listeLieux[index + 1], listeLieux[index - 1]) +
                         FloydWarshall.Distance(listeLieux[index], listeLieux[0]) -
                         FloydWarshall.Distance(listeLieux[index], listeLieux[index + 1]) -
                         FloydWarshall.Distance(listeLieux[0], listeLieux[index - 1]);
            }
            else if (index == 0)
            {
                result = distance + FloydWarshall.Distance(listeLieux[index + 1], listeLieux[listeLieux.Count-1]) +
                         FloydWarshall.Distance(listeLieux[index], listeLieux[index + 2]) -
                         FloydWarshall.Distance(listeLieux[index], listeLieux[index + 1]) -
                         FloydWarshall.Distance(listeLieux[listeLieux.Count-1], listeLieux[index + 2]);
            }
            else
            {
                result = distance + FloydWarshall.Distance(listeLieux[index+1], listeLieux[index - 1]) +
                         FloydWarshall.Distance(listeLieux[index], listeLieux[index + 2]) -
                         FloydWarshall.Distance(listeLieux[index], listeLieux[index + 1]) -
                         FloydWarshall.Distance(listeLieux[index-1], listeLieux[index + 2]);
            }
            return result;
        }

        private void TourneeMin(List<Lieu> listeLieux, int index)
        {

            int minimum = Tournee.Distance;
            int nvDistance = CalculeDistance(listeLieux, minimum, index);

            if (nvDistance < minimum)
            {
                minimum = nvDistance;
                Exchange(listeLieux, index);
            }


        }

        private void Exchange(List<Lieu> listeLieux, int index)
        {

            Lieu temp;
            if (index == listeLieux.Count-1)
            {
                temp = listeLieux[index];
                listeLieux[index] = listeLieux[0];
                listeLieux[0] = temp;
            }
            else
            {
                temp = listeLieux[index];
                listeLieux[index] = listeLieux[index + 1];
                listeLieux[index + 1] = temp;
            }

            
            Tournee.ListeLieux = listeLieux;
            sw.Stop();
            NotifyPropertyChanged("Tournee");
            sw.Start();
        }
        
    }
}