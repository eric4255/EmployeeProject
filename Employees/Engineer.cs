using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees
{
    [Serializable]
    public class Engineer : Employee
    {
        public DegreeName HighestDegree { get; set; } = DegreeName.BS;

        #region constructors 
        public Engineer() { }

        public Engineer(string firstName, string lastName, DateTime age,
                       float currPay, string ssn, DegreeName degree)
          : base(firstName, lastName, age, currPay, ssn)
        {
            // This property is defined by the Engineer class.
            HighestDegree = degree;
        }
        #endregion
        public override string Role { get { return base.Role; } }

        public override void GetSpareProp3(ref string name, ref string value)
        {
            name = "Degree:";
            value = HighestDegree.ToString();
        }
    }
}
