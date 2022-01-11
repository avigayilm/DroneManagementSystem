using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PL
{
    public class MinimumCharactersClass : ValidationRule
    {
        public int MinimumCharacters { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;
            if (charString.Length < MinimumCharacters)
                return new ValidationResult(false, $"User atleast{MinimumCharacters} characters.");
            return new ValidationResult(true, null);
        }
    }

    public class EmptyStringError : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;
            if (string.IsNullOrWhiteSpace(charString))
                return new ValidationResult(false, "can't be empty");
            return new ValidationResult(true, null);
        }
    }

    public class    longitudeCheck : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;
            if (string.IsNullOrWhiteSpace(charString))
                return new ValidationResult(false, "can't be empty");
            return new ValidationResult(true, null);
        }
    }

}
//{
//    public class InputValidation : IDataErrorInfo
//    {
//        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();
//        public string this[string name]
//        {
//            get
//            {
//                string result = null;
//                switch(name)
//                {
//                    case "Id":
//                        if (string.IsNullOrWhiteSpace(Id))
//                            result = "Id cannot be empty";
//                        else if (Id.Length() < 5)
//                            result = "Id must be  minimum of 5 characters.";
//                        break;
//                }
//                if (ErrorCollection.ContainsKey(name))
//                    ErrorCollection[name] = result;
//                else if (result != null)// we dont't have the key but we do have an error
//                    ErrorCollection.Add(name, result);
//                OnPropertyChanged("ErrorCollection");
//                return result;
//            }
//        }

//        private void OnPropertyChanged(string v)
//        {
//            throw new NotImplementedException();
//        }

//        public string Error => throw new NotImplementedException();
//    }
//}
