using System;

namespace Project.Xmltxt.DataClass
{
    [Serializable]
    public class RecaudacionBanco
    {
        private const long serialVersionUID = 8355287749132654347L;
        
        public string Banco { get; set; } = string.Empty;
        public DateTime FechaRecaudacion { get; set; }
        public short Estatus { get; set; }

        public RecaudacionBanco()
        {
            // Constructor sin parámetros
        }

        public RecaudacionBanco(string banco, DateTime fechaRecaudacion)
        {
            Banco = banco;
            FechaRecaudacion = fechaRecaudacion;
        }

        public override string ToString()
        {
            return $"Banco: {Banco}, FechaRecaudacion: {FechaRecaudacion:dd/MM/yyyy}, Estatus: {Estatus}";
        }
    }
}