using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Serialization;

using System.IO;

namespace KeyboardIdentify
{
    [Serializable]
    public class Vector
    {
        private List<double> vector;

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

        public Vector()
        {
            vector = new List<double>();
        }

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

        public override string ToString()
        {
            XmlSerializer x = new XmlSerializer(this.GetType());
            TextWriter writer = new StringWriter();
            x.Serialize(writer, this);
            return writer.ToString();
        }
    }
}
