using System;
using System.Collections.Generic;
using System.Diagnostics;
using VoyageurDeCommerce.modele.distances;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    public class AlgorithmeInsertionProche : Algorithme
    {
        public override string Nom => "Insertion Proche";

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();

            FloydWarshall.calculerDistances(listeLieux, listeRoute);

            Lieu start = listeLieux[0];
            List<Lieu> notVisited = new List<Lieu>(listeLieux);
            notVisited.Remove(start);

            Lieu end = GetFarestStore(notVisited, start);
            notVisited.Remove(end);

            Tournee.Add(start);
            Tournee.Add(end);
            sw.Stop();
            NotifyPropertyChanged("Tournee");
            sw.Start();

            Lieu toVisit = notVisited[0];

            while (notVisited.Count != 0)
            {
                toVisit = notVisited[0];

                int min = DistanceATournee(toVisit, Tournee.ListeLieux, out int ind);
                foreach (var edge in notVisited)
                {
                    int d = DistanceATournee(edge, Tournee.ListeLieux, out int i);
                    if (min > d)
                    {
                        min = d;
                        toVisit = edge;
                    }
                }

                notVisited.Remove(toVisit);
                int index;
                DistanceATournee(toVisit, Tournee.ListeLieux, out index);
                Tournee.ListeLieux.Insert(index, toVisit);
                sw.Stop();
                NotifyPropertyChanged("Tournee");
                sw.Start();
            }

            sw.Stop();
            TempsExecution = sw.ElapsedMilliseconds;
        }

        private Lieu GetFarestStore(List<Lieu> listeLieux, Lieu start)
        {
            Lieu end = listeLieux[0];
            int distanceMax = FloydWarshall.Distance(start, end);
            for (int i = 1; i < listeLieux.Count; i++)
            {
                if (FloydWarshall.Distance(start, listeLieux[i]) > distanceMax)
                {
                    distanceMax = FloydWarshall.Distance(start, listeLieux[i]);
                    end = listeLieux[i];
                }
            }

            return end;
        }

        private int Distance(Lieu s, Lieu a, Lieu b)
        {
            return FloydWarshall.Distance(a, s) + FloydWarshall.Distance(s, b)
                   - FloydWarshall.Distance(a, b);
        }

        private int DistanceATournee(Lieu s, List<Lieu> T, out int index)
        {
            int min = Distance(s, T[0], T[1]);
            index = 1;
            for (int i = 1; i < T.Count; i++)
            {
                int d = Distance(s, T[i], T[(i + 1) % T.Count]);
                if (d < min)
                {
                    min = d;
                    index = i;
                }
            }

            return min;
        }
    }
}