using System;
using System.IO;
using System.Globalization;

namespace backend_ont_2.sigecof.sql.parser
{
    public static class SqlFileLoader
    {
        // Directorio base donde se encuentran los archivos SQL
        private const string SqlDirectory = "sql";

        /// <summary>
        /// Carga un archivo SQL y reemplaza los parámetros de fecha
        /// </summary>
        /// <param name="filename">Nombre del archivo SQL (ej. "pagadas.sql")</param>
        /// <param name="desde">Fecha desde</param>
        /// <param name="hasta">Fecha hasta</param>
        /// <returns>Contenido del SQL con fechas reemplazadas</returns>

        public static string LoadFile(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("El nombre del archivo no puede estar vacío", nameof(filename));

            string path = Path.Combine(SqlDirectory, filename);
            string content = File.ReadAllText(path);
            return content;
        }
        public static string LoadFile(string filename, DateTime desde, DateTime hasta)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("El nombre del archivo no puede estar vacío", nameof(filename));

            if (desde > hasta)
                throw new ArgumentException("La fecha desde no puede ser posterior a la fecha hasta");

            // Ruta completa del archivo
            string path = Path.Combine(SqlDirectory, filename);

            if (!File.Exists(path))
                throw new IOException($"El archivo {path} no existe");

            string content = File.ReadAllText(path);

            // Formatear fechas
            string strDesde = desde.ToString("dd/MM/yyyy");
            string strHasta = hasta.ToString("dd/MM/yyyy");

            // Reemplazar parámetros
            content = content
                .Replace("TO_DATE(:PAR_DESDE,'DD/MM/RRRR')", $"TO_DATE('{strDesde}','DD/MM/YYYY')")
                .Replace("TO_DATE(:PAR_HASTA,'DD/MM/RRRR')", $"TO_DATE('{strHasta}','DD/MM/YYYY')")
                .Replace(":PAR_DESDE", $"'{strDesde}'")
                .Replace(":PAR_HASTA", $"'{strHasta}'");

            return content;
        }

        /// <summary>
        /// Versión sobrecargada que acepta fechas como strings en formato dd/MM/yyyy
        /// </summary>
        /// <param name="filename">Nombre del archivo SQL</param>
        /// <param name="strDesde">Fecha desde como string (dd/MM/yyyy)</param>
        /// <param name="strHasta">Fecha hasta como string (dd/MM/yyyy)</param>
        /// <returns>Contenido del SQL con fechas reemplazadas</returns>
        public static string LoadFile(string filename, string strDesde, string strHasta)
        {
            var culture = new CultureInfo("es-ES");
            if (!DateTime.TryParseExact(strDesde, "dd/MM/yyyy", culture, DateTimeStyles.None, out DateTime desde))
                throw new ArgumentException("Formato de fecha desde inválido. Use dd/MM/yyyy.", nameof(strDesde));

            if (!DateTime.TryParseExact(strHasta, "dd/MM/yyyy", culture, DateTimeStyles.None, out DateTime hasta))
                throw new ArgumentException("Formato de fecha hasta inválido. Use dd/MM/yyyy.", nameof(strHasta));

            return LoadFile(filename, desde, hasta);
        }
    }
}