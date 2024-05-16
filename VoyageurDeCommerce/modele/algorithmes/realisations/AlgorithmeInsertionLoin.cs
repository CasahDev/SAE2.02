using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VoyageurDeCommerce.modele.distances;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    public class AlgorithmeInsertionLoin : Algorithme
    {
        public override string Nom => "Insertion Loin";

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            // Début chrono
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();

            // Calcul distances
            FloydWarshall.calculerDistances(listeLieux, listeRoute);

            // Lieux les plus éloignés
            List<Lieu> notVisited = new List<Lieu>(listeLieux);
            GetFarestPlaces(notVisited, out Lieu start, out Lieu end);

            notVisited.Remove(start);
            notVisited.Remove(end);

            Tournee.Add(start);
            Tournee.Add(end);
            sw.Stop();
            NotifyPropertyChanged("Tournee");
            sw.Start();

            while (notVisited.Count != 0)
            {
                Lieu toVisit = notVisited[0];

                // Calcule le plus petit écart de distance à la tournée
                int max = DistanceATournee(toVisit, Tournee.ListeLieux, out int index);
                foreach (Lieu place in notVisited)
                {
                    int d = DistanceATournee(place, Tournee.ListeLieux, out int ind);
                    if (d > max)
                    {
                        toVisit = place;
                        max = d;
                        index = ind;
                    }
                }

                // Ajoute
                notVisited.Remove(toVisit);
                Tournee.ListeLieux.Insert(index, toVisit);

                sw.Stop();
                NotifyPropertyChanged("Tournee");
                sw.Start();
            }

            sw.Stop();
            TempsExecution = sw.ElapsedMilliseconds;
        }

        private void GetFarestPlaces(List<Lieu> listeLieux, out Lieu start, out Lieu end)
        {
            start = listeLieux[0];
            end = listeLieux[1];
            int distanceMax = FloydWarshall.Distance(start, end);
            for (int i = 1; i < listeLieux.Count; i++)
            {
                for (int j = i + 1; j < listeLieux.Count; j++)
                {
                    int d = FloydWarshall.Distance(listeLieux[i], listeLieux[j]);
                    if (d > distanceMax)
                    {
                        distanceMax = d;
                        start = listeLieux[i];
                        end = listeLieux[j];
                    }
                }

            }
        }

        private int DistanceATournee(Lieu s, List<Lieu> T, out int index)
        {
            int min = s.Distance(T[0], T[1]);
            int ind = 1;
            for (int i = 0; i < Tournee.ListeLieux.Count; i++)
            {
                int d = s.Distance(Tournee.ListeLieux[i],
                    Tournee.ListeLieux[(i + 1) % Tournee.ListeLieux.Count]);
                if (d < min)
                {
                    min = d;
                    ind = i + 1;
                }
            }

            index = ind;
            return min;
        }
    }
}