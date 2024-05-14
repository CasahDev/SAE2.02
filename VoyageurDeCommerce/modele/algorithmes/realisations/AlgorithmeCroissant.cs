using System.Collections.Generic;
using System.Diagnostics;
using VoyageurDeCommerce.modele.distances;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    public class AlgorithmeCroissant : Algorithme
    {
        public override string Nom => "Tournée croissante";

        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();

            FloydWarshall.calculerDistances(listeLieux, listeRoute);

            foreach (var lieu in listeLieux)
            {
                Tournee.Add(lieu);
                sw.Stop();
                NotifyPropertyChanged("Tournee");
                sw.Start();
            }
            sw.Stop();
            TempsExecution = sw.ElapsedMilliseconds;
        }
    }
}