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

            Lieu depart = listeLieux[0];
            Lieu arrivee = GetFarest(listeLieux, depart);
            List<Lieu> nonVisitee = new List<Lieu>(listeLieux);
            nonVisitee.Remove(depart);

            Tournee.Add(depart);
            Tournee.Add(arrivee);
            sw.Stop();
            NotifyPropertyChanged("Tournee");
            sw.Start();

            Lieu aVisiter = listeLieux[0];


            while (aVisiter != null)
            {
                Lieu lieu = aVisiter;
                aVisiter = null;
                nonVisitee.Remove(lieu);

                int min = FloydWarshall.Distance(lieu, nonVisitee[0]);
                foreach (var voisin in nonVisitee)
                {
                    if (min > FloydWarshall.Distance(lieu, voisin))
                    {
                        aVisiter = voisin;
                    }
                }

                int index = GetIndex(lieu);
                Tournee.ListeLieux.Insert(index, lieu);
                sw.Stop();
                NotifyPropertyChanged("Tournee");
                sw.Start();
            }

            sw.Stop();
            TempsExecution = sw.ElapsedMilliseconds;
        }

        private int GetIndex(Lieu lieu)
        {
            int index = Tournee.ListeLieux.Count - 1;

            Tournee.Add(lieu);
            int dMax = Tournee.Distance;
            for (int i = 1; i < Tournee.ListeLieux.Count; i++)
            {
                Tournee.ListeLieux.Remove(lieu);
                Tournee.ListeLieux.Insert(i, lieu);
                if (dMax > Tournee.Distance)
                {
                    dMax = Tournee.Distance;
                    index = i;
                }
            }

            return index;
        }

        private Lieu GetFarest(List<Lieu> listeLieux, Lieu depart)
        {
            depart = listeLieux[0];
            Lieu arrivee = listeLieux[1];
            int distanceMax = FloydWarshall.Distance(depart, arrivee);
            for (int i = 0; i < listeLieux.Count; i++)
            {
                if (FloydWarshall.Distance(listeLieux[i], listeLieux[i]) > distanceMax)
                {
                    distanceMax = FloydWarshall.Distance(depart, listeLieux[i]);
                    arrivee = listeLieux[i];
                }
            }

            return arrivee;
        }
    }
}