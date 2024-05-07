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
            List<Lieu> nonVisitee = new List<Lieu>(listeLieux);

            Tournee.Add(listeLieux[0]);
            nonVisitee.RemoveAt(0);

            sw.Stop();
            NotifyPropertyChanged("Tournee");
            sw.Start();

            Lieu aVisiter = listeLieux[1];


            while (aVisiter != null)
            {
                Lieu lieu = aVisiter;
                aVisiter = null;
                nonVisitee.Remove(lieu);
                Tournee.Add(lieu);
                sw.Stop();
                NotifyPropertyChanged("Tournee");
                sw.Start();

                int min = FloydWarshall.Distance(lieu, nonVisitee[1]);
                foreach (var voisin in nonVisitee)
                {
                    if (min > FloydWarshall.Distance(lieu, voisin))
                    {
                        aVisiter = voisin;
                    }
                }

            }
            sw.Stop();
            this.TempsExecution = sw.ElapsedMilliseconds;
        }
    }
}