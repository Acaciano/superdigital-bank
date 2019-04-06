using System.Collections.Generic;

namespace SuperDigital.DigitalAccount.Application.Validation
{
    public class ValidationApplicationResult
    {
        private readonly List<ValidationApplicationError> _errors = new List<ValidationApplicationError>();

        public string Mensagem { get; set; }

        public bool IsValid
        {
            get { return _errors.Count == 0; }
            set
            {
                var b = value;
            }
        }

        public ICollection<ValidationApplicationError> Erros { get { return this._errors; } }
    }
}
