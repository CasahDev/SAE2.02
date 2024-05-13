using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using VoyageurDeCommerce.modele.distances;
using VoyageurDeCommerce.modele.lieux;

namespace VoyageurDeCommerce.modele.algorithmes.realisations
{
    /// <summary>
    /// Classe de l'algorithme Dijkstra
    /// </summary>
    public class AlgorithmeDijkstra : Algorithme
    {
        private Dictionary<Lieu, double> _distances;
        private Dictionary<Lieu, bool> _estVisite;
        private Dictionary<Lieu, Lieu> _predecesseur;
        
        /// <summary>
        /// Propriété donnant le nom de l'algorithme
        /// </summary>
        public override string Nom => "Dijsktra";

        /// <summary>
        /// Constructeur de Dijkstra
        /// </summary>
        public AlgorithmeDijkstra()
        {
            _distances = new Dictionary<Lieu, double>();
            _estVisite = new Dictionary<Lieu, bool>();
            _predecesseur = new Dictionary<Lieu, Lieu>(); 
        }
        
        /// <summary>
        /// Méthode de l'initialisation de l'algorithme
        /// </summary>
        /// <param name="listeLieux">La Liste des lieux a visité</param>
        private void Initialisation(List<Lieu> listeLieux)
        {
            foreach(Lieu l in listeLieux)
            {
                _distances[l] = Double.PositiveInfinity;
                _estVisite[l] = false;
                _predecesseur[l] = null;
            }
            _distances[listeLieux[0]] = 0;
        }
        
        /// <summary>
        /// Méthode qui permet de mettre a jour les distances et les predecesseurs
        /// </summary>
        /// <param name="a">Lieu de départ</param>
        /// <param name="b">Lieu d'arrivée</param>
        private void Relachement(Lieu a, Lieu b)
        {
            if (_distances[b] > _distances[a] + FloydWarshall.Distance(a, b))
            {
                _distances[b] = _distances[a] + FloydWarshall.Distance(a, b);
                _predecesseur[b] = a;
            }
        }

        /// <summary>
        /// Méthode donnant les voisins d'un lieu
        /// </summary>
        /// <param name="lieu">le lieu de départ</param>
        /// <param name="route">la liste de toutes les routes</param>
        /// <returns>La liste de tous les voisins de notre lieu</returns>
        private List<Lieu> RechercheVoisin(Lieu lieu, List<Route> route)
        {
            List<Lieu> voisins = new List<Lieu>();
            foreach (Route r in route)
            {
                if (lieu == r.Depart)
                {
                    voisins.Add(r.Arrivee);
                }

                if (lieu == r.Arrivee)
                {
                    voisins.Add(r.Depart);
                }
            }
            return voisins;
        }
        
        /// <summary>
        /// Méthode qui donne la distance minimale des lieux
        /// </summary>
        /// <param name="listeLieu"></param>
        /// <returns></returns>
        private Lieu Minimum(List<Lieu> listeLieu)
        {
            Lieu rep = listeLieu[1];
            double min = _distances[rep];
            foreach (Lieu l in listeLieu)
            {
                if (!_estVisite[l])
                {
                    rep = l;
                    min = _distances[l];
                }
            }
            for (int i = 0; i < listeLieu.Count(); i++)
            {
                if (_distances[listeLieu[i]] < min && !_estVisite[listeLieu[i]])
                {
                    min = _distances[listeLieu[i]];
                    rep = listeLieu[i];
                }
            }
            //Console.WriteLine("min = " + min);
            return rep;
        }

        /// <summary>
        /// Méthode qui lance l'aglorithme de Dijkstra et calcule les PCC de chaque point par rapport au point de départ
        /// </summary>
        /// <param name="listeLieux">La Liste des Lieux</param>
        /// <param name="listeRoute">La liste des routes</param>
        private void Dijkstra(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            //Initialisation
            Initialisation(listeLieux);


            //Calcul
            while (_estVisite.ContainsValue(false))
            {
                Lieu a = Minimum(listeLieux);
                _estVisite[a] = true;
                List<Lieu> voisins = RechercheVoisin(a, listeRoute);
                foreach (Lieu b in voisins)
                {
                    Relachement(a, b);
                }
            }
        }

        /// <summary>
        /// Méthode executant l'algorithme
        /// </summary>
        /// <param name="listeLieux">La Liste des Lieux</param>
        /// <param name="listeRoute">La liste des routes</param>
        public override void Executer(List<Lieu> listeLieux, List<Route> listeRoute)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            
            FloydWarshall.calculerDistances(listeLieux, listeRoute);
            Dijkstra(listeLieux,listeRoute);
            sw.Stop();
            this.TempsExecution = sw.ElapsedMilliseconds;
        }
    }
}