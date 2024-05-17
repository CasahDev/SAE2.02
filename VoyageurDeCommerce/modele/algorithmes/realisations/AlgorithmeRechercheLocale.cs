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
            sw = Stopwatch.StartNew();
            sw.Start();

            FloydWarshall.calculerDistances(listeLieux, listeRoute);
            Algorithme algo = new AlgorithmeCroissant();
            algo.Executer(listeLieux, listeRoute);


            foreach (Lieu lieu in algo.Tournee.ListeLieux)
            {
                Tournee.ListeLieux.Add(lieu);
                sw.Stop();
                NotifyPropertyChanged("Tournee");
                sw.Start();
            }

            bool gotBetterSolution = true;
            int maxAttempt = 0;
            while (gotBetterSolution && maxAttempt < 10)
            {
                gotBetterSolution = ChercherVoisine();
                maxAttempt++;
            }

            sw.Stop();
            TempsExecution = sw.ElapsedMilliseconds;
        }

        private bool ChercherVoisine()
        {
            Tournee tournee = new Tournee(Tournee);

            for (int i = 0; i < Tournee.ListeLieux.Count; i++)
            {
                int min = Tournee.Distance;
                int nvDistance = CalculeDistance(Tournee.ListeLieux, min, i);

                if (nvDistance < min)
                {
                    Exchange(Tournee.ListeLieux, i);
                }
            }

            return tournee != Tournee;
        }

        private int CalculeDistance(List<Lieu> listeLieux, int distance, int index)
        {

            int result = distance +
                         FloydWarshall.Distance(listeLieux[Mod(index + 1, listeLieux.Count)],
                             listeLieux[Mod(index - 1, listeLieux.Count)]) +
                         FloydWarshall.Distance(listeLieux[index], listeLieux[Mod(index + 2, listeLieux.Count)]) -
                         FloydWarshall.Distance(listeLieux[Mod(index - 1, listeLieux.Count)], listeLieux[index]) -
                         FloydWarshall.Distance(listeLieux[Mod(index + 1, listeLieux.Count)],
                             listeLieux[Mod(index + 2, listeLieux.Count)]);

            return result;
        }

        private void Exchange(List<Lieu> listeLieux, int index)
        {
            (listeLieux[index], listeLieux[Mod(index + 1, listeLieux.Count)]) =
                (listeLieux[Mod((index + 1), listeLieux.Count)], listeLieux[index]);

            sw.Stop();
            NotifyPropertyChanged("Tournee");
            sw.Start();
        }

        private int Mod(int x, int m) {
            return (x%m + m)%m;
        }
    }
}