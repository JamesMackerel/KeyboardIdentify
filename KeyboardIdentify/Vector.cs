﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Serialization;

using System.IO;

namespace KeyboardIdentify
{
    /// <summary>
    /// Represent a vector.
    /// </summary>
    public class Vector 
    {
        public List<double> vector;

        public double this[int index]
        {
            get
            {
                return vector[index];
            }
            set
            {
                vector[index] = value;
            }
        }

        public int Dimension
        {
            get
            {
                return vector.Count;
            }
        }

        public Vector()
        {
            vector = new List<double>();
        }

        public Vector(double[] array)
        {
            vector = new List<double>(array);
        }

        public Vector(ICollection<double> array)
        {
            vector = new List<double>();
            foreach(double i in array)
            {
                vector.Add(i);
            }
        }

        /// <summary>
        /// Caculate distance between 2 vectors.
        /// </summary>
        /// <param name="v">another vector</param>
        /// <returns>distance</returns>
        public double DistanceBetween(Vector v)
        {
            if(v.Dimension != Dimension)
            {
                throw new ArgumentException("Dimension not equal!");
            }
            double distance = 0;
            for (int iter = 0; iter < Dimension; ++iter)
            {
                distance += Math.Pow(this[iter] + v[iter], 2);
            }
            return Math.Sqrt(distance);
        }

        /// <summary>
        /// deserialize a vector from xml
        /// </summary>
        /// <param name="reader">a XmlReader</param>
        /// <returns>a vector object</returns>
        public static Vector GetVectorFromXml(XmlReader reader)
        {
            XmlSerializer x = new XmlSerializer(typeof(Vector));
            Vector v;
            try
            {
                v = x.Deserialize(reader) as Vector;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }

            return v;
        }

        /// <summary>
        /// serizlize a vector object to xml document
        /// </summary>
        /// <returns>a string object that contains a serialized vector object.</returns>
        public override string ToString()
        {
            XmlSerializer x = new XmlSerializer(this.GetType());
            TextWriter writer = new StringWriter();
            x.Serialize(writer, this);
            return writer.ToString();
        }
    }
}
