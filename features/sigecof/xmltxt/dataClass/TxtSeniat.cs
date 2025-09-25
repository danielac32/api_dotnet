using System;
using System.Text;

namespace backend_ont_2.Xmltxt.DataClass
{
    [Serializable]
    public class TxtSeniat
    {
        public string Organismo { get; set; } = string.Empty;
        public string Banco { get; set; } = string.Empty;
        public string Agencia { get; set; } = string.Empty;
        public string Rif { get; set; } = string.Empty;
        public string Planilla { get; set; } = string.Empty;
        public DateTime FechaRecaudacion { get; set; }
        public int? TipoTransaccion { get; set; } = 2; // Valor por defecto
        public string Forma { get; set; } = string.Empty;
        public double? Efectivo { get; set; }
        public double? OtrosPagos { get; set; }
        public string Seguridad { get; set; } = string.Empty;
        public string Safe { get; set; } = string.Empty;
        public int? Estado { get; set; }
        public int? Anho { get; set; }
        public int? LoteSeq { get; set; }
        public int? PlanSeq { get; set; }

        public TxtSeniat(string banco, string agencia, string rif, string planilla, DateTime fechaRecaudacion, 
                        string forma, double? efectivo, double? otrosPagos, string seguridad, string safe)
        {
            Organismo = !string.IsNullOrEmpty(forma) && forma.Length >= 2 ? forma.Substring(0, 2) : "";
            Banco = banco;
            Agencia = agencia;
            Rif = rif;
            Planilla = planilla;
            FechaRecaudacion = fechaRecaudacion;
            TipoTransaccion = 2;
            Forma = forma;
            Efectivo = efectivo;
            OtrosPagos = otrosPagos;
            Seguridad = seguridad;
            Safe = safe;
        }

        public TxtSeniat()
        {
            // Constructor sin parámetros
        }

        public override bool Equals(object? obj)
        {
            if (obj is not TxtSeniat that) return false;

            return string.Equals(Agencia, that.Agencia) && 
                   string.Equals(Banco, that.Banco) &&
                   TipoTransaccion == that.TipoTransaccion &&
                   string.Equals(Planilla, that.Planilla) &&
                   string.Equals(Safe, that.Safe) &&
                   string.Equals(Seguridad, that.Seguridad);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Agencia, Banco, TipoTransaccion, Planilla, Safe, Seguridad);
        }

        public override string ToString()
        {
            return $@"
                --- Datos de Transacción SENIAT ---
                Organismo: {Organismo}
                Banco: {Banco}
                Agencia: {Agencia}
                RIF Contribuyente: {Rif}
                Número Planilla: {Planilla}
                Fecha Recaudación: {FechaRecaudacion:dd/MM/yyyy}
                Tipo Transacción: {TipoTransaccion ?? 0}
                Forma: {Forma}
                Efectivo: {Efectivo ?? 0.0}
                Otros Pagos: {OtrosPagos ?? 0.0}
                Seguridad: {Seguridad}
                Safe: {Safe}
                Estado: {Estado}
                Año: {Anho}
                Secuencia Lote: {LoteSeq}
                Secuencia Planilla: {PlanSeq}";
        }
    }
}