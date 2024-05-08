using System;
using System.Collections.Generic;
using System.Diagnostics;
using VoyageurDeCommerce.modele.distances;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    public class AlgorithmePlusProcheVoisin : Algorithme
    {
        public override string Nom => "Plus proche voisin";

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();

            FloydWarshall.calculerDistances(listeLieux, listeRoute);
            List<Lieu> notVisited = new List<Lieu>(listeLieux);

            Tournee.Add(listeLieux[0]);
            notVisited.RemoveAt(0);

            sw.Stop();
            NotifyPropertyChanged("Tournee");
            sw.Start();

            Lieu toVisit = listeLieux[1];
            Lieu lastVisited = Tournee.ListeLieux[Tournee.ListeLieux.Count - 1];


            while (notVisited.Count != 0)
            {
                toVisit = notVisited[0];

                int min = FloydWarshall.Distance(lastVisited, toVisit);
                foreach (var neighbor in notVisited)
                {
                    int d = FloydWarshall.Distance(lastVisited, neighbor);
                    if (min > d)
                    {
                        min = d;
                        toVisit = neighbor;
                    }
                }

                lastVisited = toVisit;
                notVisited.Remove(toVisit);
                Tournee.Add(toVisit);
                sw.Stop();
                NotifyPropertyChanged("Tournee");
                sw.Start();

            }
            sw.Stop();
            this.TempsExecution = sw.ElapsedMilliseconds;
        }
    }
}