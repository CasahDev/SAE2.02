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
            // Début chrono
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();

            FloydWarshall.calculerDistances(listeLieux, listeRoute);

            // Usine
            Lieu start = listeLieux[0];
            List<Lieu> notVisited = new List<Lieu>(listeLieux);
            notVisited.Remove(start);

            // Lieu le plus éloigné
            Lieu end = GetFarestStore(notVisited, start);
            notVisited.Remove(end);

            Tournee.Add(start);
            sw.Stop();
            NotifyPropertyChanged("Tournee");
            sw.Start();

            Tournee.Add(end);
            sw.Stop();
            NotifyPropertyChanged("Tournee");
            sw.Start();

            while (notVisited.Count != 0)
            {
                var toVisit = notVisited[0];

                // Calcule le plus petit écart de distance à la tournée
                int min = DistanceATournee(toVisit, Tournee.ListeLieux, out _);
                foreach (var place in notVisited)
                {
                    int d = DistanceATournee(place, Tournee.ListeLieux, out _);
                    if (min > d)
                    {
                        min = d;
                        toVisit = place;
                    }
                }

                // Ajoute
                notVisited.Remove(toVisit);
                int distance = DistanceATournee(toVisit, Tournee.ListeLieux, out int index);
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
                    Console.WriteLine(T[i] + " - " + index);
                }
            }

            return min;
        }
    }
}