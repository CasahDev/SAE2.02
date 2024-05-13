using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Documents;
using VoyageurDeCommerce.modele.distances;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    public class AlgorithmeInsertionLoin : Algorithme
    {
        private List<Lieu> nonVisites;
        private Stopwatch sw;
        public override string Nom => "Insertion loin";

        /// <summary>
        /// Définit l'exécution de l'algorithme
        /// </summary>
        /// <param name="listeLieux">Les différents lieux du graphe</param>
        /// <param name="listeRoute">Les différentes routes du graphe</param>
        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            sw = Stopwatch.StartNew();
            sw.Start();
            FloydWarshall.calculerDistances(listeLieux,listeRoute);
            
            this.nonVisites = new List<Lieu>(listeLieux);

            Lieu start = listeLieux[0];
            
            this.nonVisites.Remove(start);

            Lieu end = FarthestFromStore(listeLieux);
            this.nonVisites.Remove(end);
            
            Tournee.Add(start);
            Tournee.Add(end);

            sw.Stop();
            NotifyPropertyChanged("Tournee");
            sw.Start();
            
            SmallestInsertionTournee();

            TempsExecution = sw.ElapsedMilliseconds;
        }

        private Lieu FarthestFromStore(List<Lieu> listeLieux)
        {
            int maxDist = 0;
            Lieu result = null;
            foreach (Lieu lieu in listeLieux)
            {
                if (FloydWarshall.Distance(listeLieux[0],lieu)>maxDist)
                {
                    maxDist = FloydWarshall.Distance(listeLieux[0], lieu);
                    result = lieu;
                }
            }

            this.nonVisites.Remove(result);
            return result;
        }
        
        private int DistanceFromTournee(List<Lieu> listeLieux,Lieu sommet)
        {
            int result;
            if (listeLieux.Count>2)
            {
                List<int> distances = new List<int>();
                List<Lieu> couple = new List<Lieu>();
                couple.Add(listeLieux[0]);
                couple.Add(listeLieux[1]);
                for (int i = 1; i < listeLieux.Count-2; i++)
                {
                    couple[0] = listeLieux[i];
                    couple[1] = listeLieux[i+1];
                    distances.Add(DistanceFromCouple(sommet,couple));
                }

                couple[0] = listeLieux[listeLieux.Count - 1];
                couple[1] = listeLieux[0];
                distances.Add(DistanceFromCouple(sommet,couple));
                result = distances.Min();
            }
            else
            {
                result = DistanceFromCouple(sommet, listeLieux);
            }

            return result;

        }

        private int DistanceFromCouple(Lieu sommet, List<Lieu> couple)
        {
            return FloydWarshall.Distance(sommet, couple[0]) + FloydWarshall.Distance(sommet, couple[1]) -
                   FloydWarshall.Distance(couple[0], couple[1]);
        }

        private Lieu FarthestFromTournee()
        {
            List<Lieu> nVisiteCopy = new List<Lieu>(this.nonVisites);
            Lieu result = null;
            int max = 0;
            while (this.nonVisites.Count != 0)
            {
                foreach (Lieu lieu in nVisiteCopy)
                {
                    if (DistanceFromTournee(nVisiteCopy,lieu)>max)
                    {
                        max = DistanceFromTournee(this.nonVisites,lieu);
                        result = lieu;
                    }

                    this.nonVisites.Remove(lieu);
                }
            }

            return result;
        }

        private void SmallestInsertionTournee()
        {
            int minDist = Tournee.Distance;
            Tournee tourneeTest = new Tournee(Tournee);

            for (int i = 0; i < tourneeTest.ListeLieux.Count; i++)
            {
                tourneeTest.ListeLieux.Insert(i,FarthestFromTournee());
                sw.Stop();
                NotifyPropertyChanged("Tournee");
                sw.Start();
                
                if (tourneeTest.Distance < minDist)
                {
                    minDist = tourneeTest.Distance;
                    Tournee = tourneeTest;
                }
            }
            sw.Stop();
        } 
    }
}