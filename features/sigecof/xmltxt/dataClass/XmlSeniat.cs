using System;
using System.Text;

namespace backend_ont_2.Xmltxt.DataClass
{
    [Serializable]
    public class XmlSeniat
    {
        private const long serialVersionUID = -1673793401724749472L;
        
        // Propiedades auto-implementadas
        public string Banco { get; set; } = string.Empty;
        public string Agencia { get; set; } = string.Empty;
        public string Safe { get; set; } = string.Empty;
        public string Seguridad { get; set; } = string.Empty;
        public DateTime FechaTransmision { get; set; }
        public DateTime FechaRecaudacion { get; set; }
        public string Rif { get; set; } = string.Empty;
        public string Forma { get; set; } = string.Empty;
        public string Planilla { get; set; } = string.Empty;
        public string Periodo { get; set; } = string.Empty;
        public string Aduana { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Electronico { get; set; } = string.Empty;
        public double? Monto { get; set; }
        public double? Efectivo { get; set; }
        public double? Cheque { get; set; }
        public double? Titulo { get; set; }
        public double? Cert { get; set; }
        public double? Bono { get; set; }
        public double? Dpn { get; set; }
        public string Partida { get; set; } = string.Empty;
        public double? MontoPartida { get; set; }
        public int Estatus { get; set; }

        public XmlSeniat(string banco, string agencia, string rif, string planilla,
                        DateTime fechaRecaudacion, string forma, double? efectivo,
                        double? otrosPagos, string seguridad, string safe)
        {
            Banco = banco;
            Agencia = agencia;
            Rif = rif;
            Planilla = planilla;
            FechaRecaudacion = fechaRecaudacion;
            Forma = forma;
            Efectivo = efectivo;
            // otrosPagos podría asignarse a alguna propiedad específica
            Seguridad = seguridad;
            Safe = safe;
        }

        public XmlSeniat()
        {
            // Constructor sin parámetros
        }


 
        public override string ToString()
        {
            return $@"
            --- Planilla de Pago ---
            Banco: {Banco ?? ""}
            Agencia: {Agencia ?? ""}
            Safe: {Safe ?? ""}
            Seguridad Planilla: {Seguridad ?? ""}
            Fecha Transmisión: {(FechaTransmision != default(DateTime) ? FechaTransmision.ToString("dd/MM/yyyy") : "")}
            Fecha Recaudación: {(FechaRecaudacion != default(DateTime) ? FechaRecaudacion.ToString("dd/MM/yyyy") : "")}
            RIF Contribuyente: {Rif ?? ""}
            Código Forma: {Forma ?? ""}
            Número Planilla: {Planilla ?? ""}
            Periodo: {Periodo ?? ""}
            Aduana: {Aduana ?? ""}
            Región: {Region ?? ""}
            Cancelado Electrónicamente: {Electronico ?? ""}
            Monto Total: {Monto ?? 0.0}
            Monto Efectivo: {Efectivo ?? 0.0}
            Monto Cheque: {Cheque ?? 0.0}
            Monto Títulos: {Titulo ?? 0.0}
            Monto Certificados: {Cert ?? 0.0}
            Monto Bonos Export: {Bono ?? 0.0}
            Monto Bonos DPN: {Dpn ?? 0.0}
            Partida: {Partida ?? ""}
            Monto Partida: {MontoPartida ?? 0.0}
            Estatus: {Estatus}";
        }
    }
}