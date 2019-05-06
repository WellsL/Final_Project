﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Final_Project
{
    public abstract class Vehicule
    {

        private int nVehicule;
        private readonly string marque;
        private readonly string modele;
        private int km;
        private string couleur;
        private bool isDisponible;

        /**
         * Constructeur pour vehicule d'occasion
         */

        protected Vehicule(int nVehicule, string marque, string modele, int km, string couleur)
        {
            this.nVehicule = nVehicule;
            this.marque = marque;
            this.modele = modele;
            this.km = km;
            this.couleur = couleur;
            this.isDisponible = true;
        }

        
        /* Properties */

        public string Marque => marque;
        public string Modele => modele;
 
        public int Km { get => km; set => km = value; }
        public string Couleur { get => couleur; set => couleur = value; }
        public bool IsDisponible { get => isDisponible; set => isDisponible = value; }
        public int NVehicule { get => nVehicule; set => nVehicule = value; }
    }
}