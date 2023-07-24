using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Helps
{
    public class DataValidations
    {
        private ValidationContext validationContext;
        private List<ValidationResult> validationResults;
        private bool valid;
        private string message;

        public DataValidations(object instance)
        {
            validationContext = new ValidationContext(instance);
            validationResults = new List<ValidationResult>();
            valid = Validator.TryValidateObject(instance, validationContext, validationResults, true);
        }

        public bool Validate()
        {
            if(!valid)
            {
                foreach (ValidationResult item in validationResults)
                {
                    message += item.ErrorMessage + "\n";
                }
                System.Windows.Forms.MessageBox.Show(message);
            }
            return valid;
        }
    }
}
