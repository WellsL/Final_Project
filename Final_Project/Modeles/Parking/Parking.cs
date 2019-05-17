﻿
using System;
using System.Collections.Generic;
using Final_Project.Exceptions;

namespace Final_Project.Parking
{
    public class Parking
    {
        private readonly Place[] places;
        public const int NbPlaces = 10;

        public Parking(Action<Place[], Parking> remplirTab)
        {
            places = new Place[NbPlaces];
            remplirTab(places, this);
        }

        /* Properties */

        public Place[] Places => places;
        public bool IsPlein
        {
            get
            {
                int i = 0;
                while (i < NbPlaces && !places[i].IsDisponible) { i++; }

                return i == NbPlaces && !places[NbPlaces-1].IsDisponible;
            }
        }

        /* Public Methodes */

        public Place GetPlace(int numPlace)
        {
            return places[numPlace];
        }

        public Place GetPlaceDisp()
        {
            int i = 0;
            while (!places[i].IsDisponible && i < NbPlaces) { i++; }

            return i == NbPlaces & !places[i - 1].IsDisponible ? null : places[i - 1];
        }

        public List<Place> GetPlacesDisp()
        {
            List<Place> lPlaces = new List<Place>();
            for (int i = 0; i < places.Length; i++)
            {
                if (places[i].IsDisponible)
                    lPlaces.Add(places[i]);
            }

            return lPlaces.Count == 0 ? null : lPlaces;
        }

    }
}