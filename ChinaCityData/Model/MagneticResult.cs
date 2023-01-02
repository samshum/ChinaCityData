using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChinaCityData.Model
{
    public class MagneticResult
    {
        public Result[] result {get;set;}

        public string model { get;set;}

        public Units units { get;set;}

        public string version { get;set;}
    }

    public class Result { 
      public double date{ get;set;}
      public double totalintensity_uncertainty{ get;set;}
      public double horintensity{ get;set;}
      public double latitude{ get;set;}
      public double zcomponent{ get;set;}
      public double horintensity_uncertainty{ get;set;}
      public double ycomponent_uncertainty{ get;set;}
      public double inclination_uncertainty{ get;set;}
      public double declination_sv{ get;set;}
      public double totalintensity{ get;set;}
      public double longitude{ get;set;}
      public double elevation{ get;set;}
      public double inclination{ get;set;}
      public double ycomponent{ get;set;}
      public double totalintensity_sv{ get;set;}
      public double declination_uncertainty{ get;set;}
      public double zcomponent_sv{ get;set;}
      public double declination{ get;set;}
      public double ycomponent_sv{ get;set;}
      public double xcomponent_uncertainty{ get;set;}
      public double xcomponent_sv{ get;set;}
      public double xcomponent{ get;set;}
      public double inclination_sv{ get;set;}
      public double zcomponent_uncertainty{ get;set;}
      public double horintensity_sv{ get;set;}
    }

    public class Units
    { 
    public string inclination { get;set;}
    public string totalintensity_uncertainty { get;set;}
    public string elevation { get;set;}
    public string ycomponent { get;set;}
    public string totalintensity_sv { get;set;}
    public string horintensity { get;set;}
    public string latitude { get;set;}
    public string zcomponent { get;set;}
    public string declination_uncertainty { get;set;}
    public string horintensity_uncertainty { get;set;}
    public string zcomponent_sv { get;set;}
    public string declination { get;set;}
    public string ycomponent_sv { get;set;}
    public string xcomponent_uncertainty { get;set;}
    public string ycomponent_uncertainty { get;set;}
    public string inclination_uncertainty { get;set;}
    public string declination_sv { get;set;}
    public string xcomponent_sv { get;set;}
    public string xcomponent { get;set;}
    public string totalintensity { get;set;}
    public string inclination_sv { get;set;}
    public string zcomponent_uncertainty { get;set;}
    public string horintensity_sv { get;set;}
    public string longitude { get; set; }
    }
}
