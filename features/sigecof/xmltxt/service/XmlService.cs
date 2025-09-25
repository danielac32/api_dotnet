
//using System.Net.Mime;
using System.Globalization;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.AspNetCore.Hosting;
using backend_ont_2.Xmltxt.DataClass;

namespace backend_ont_2.Xmltxt.Service;

public class XmlService
{


    private readonly IWebHostEnvironment _environment;
    public XmlService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }



    public async Task<Dictionary<string, object>>ProcessXmlFiles()
    {
        var rootPath = _environment.ContentRootPath;
        var xmlFolderPath = Path.Combine(rootPath, "xml");

        if (!Directory.Exists(xmlFolderPath))
        {
            throw new InvalidOperationException("La ruta proporcionada no es un directorio válido.");
        }
        var xmlFiles = Directory.GetFiles(xmlFolderPath, "*.xml")
               .Select(f => new FileInfo(f))
               .Where(f => f.Exists)
               .ToList();

        if (!xmlFiles.Any())
        {
            //throw new InvalidOperationException("⚠️ No se encontraron archivos XML.");
            return new Dictionary<string, object>
            {
                ["resultados"] = new List<ProcessResult>(),
                ["total_planillas"] = 0,
                ["total_errores"] = 0,
                ["mensaje"] = "No hay archivos XML para procesar."
            };
        }

        int totalPlanillas = 0;
        int totalErrores = 0;
        var resultados = new List<Dictionary<string, object>>();        
        foreach (var file in xmlFiles)
            {
                Console.WriteLine($"🔍 Procesando archivo: {file.Name}");

                try
                {
                    var resultado = await ProcessXmlFile(file.FullName);

                    int planillas = resultado.GetValueOrDefault("planillas", 0);
                    int errores = resultado.GetValueOrDefault("errores", 0);

                    totalPlanillas += planillas;
                    totalErrores += errores;

                    resultados.Add(new Dictionary<string, object>
                    {
                        ["archivo"] = file.Name,
                        ["planillas"] = planillas,
                        ["errores"] = errores
                    });

                    // Simular borrado
                    File.Delete(file.FullName);
                    Console.WriteLine($"🗑️ Archivo eliminado: {file.Name}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error crítico con archivo {file.Name}: {ex.Message}");
                    totalErrores++;

                    resultados.Add(new Dictionary<string, object>
                    {
                        ["archivo"] = file.Name,
                        ["planillas"] = 0,
                        ["errores"] = 1,
                        ["detalle"] = ex.Message
                    });
                }
            }
            return new Dictionary<string, object>
            {
                ["resultados"] = resultados,
                ["total_planillas"] = totalPlanillas,
                ["total_errores"] = totalErrores
            };
            // Mostrar resumen final (como respuesta JSON en Java)
            /*Console.WriteLine("\n=== RESUMEN FINAL ===");
            Console.WriteLine($"Total planillas procesadas: {totalPlanillas}");
            Console.WriteLine($"Total errores: {totalErrores}");
            Console.WriteLine("Resultados por archivo:");
            foreach (var r in resultados)
            {
                Console.WriteLine($"  - {r["archivo"]} | Planillas: {r["planillas"]}, Errores: {r["errores"]}");
            }*/
    }


    private async Task<Dictionary<string, int>> ProcessXmlFile(string filePath)
        {
            int planillasProcesadas = 0;
            int errores = 0;

            try
            {
                var doc = XDocument.Load(filePath);
                var planillas = doc.Descendants("Planilla_Pago");

                foreach (var elemento in planillas)
                {
                    try
                    {
                        var xml = ArmarCabeceraPlanilla(elemento);
                        
                        // Filtros como en Java
                        if (new[] { "999", "000" }.Contains(xml.Banco) ||
                            new[] { "79084", "99084", "00084" }.Contains(xml.Forma))
                        {
                            continue; // Saltar estos casos
                        }

                        var txt = CrearTxt(xml);

                        // Simulación de "inserción"
                        //Console.WriteLine("✅ TXT generado para inserción:");
                        Console.WriteLine(txt.ToString());

                        planillasProcesadas++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Error al procesar una planilla en {filePath}: {ex.Message}");
                        errores++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al cargar XML {filePath}: {ex.Message}");
                errores++;
            }

            return new Dictionary<string, int>
            {
                ["planillas"] = planillasProcesadas,
                ["errores"] = errores
            };
        }

    private XmlSeniat ArmarCabeceraPlanilla(XElement element)
        {
            var xml = new XmlSeniat();

            DateTime TryParseDate(string value, string format = "dd/MM/yyyy")
            {
                return DateTime.TryParseExact(value, format, null, DateTimeStyles.None, out var dt) ? dt : default;
            }

            double? TryParseDouble(string value)
            {
                return double.TryParse(value.Replace(",", "."), out var d) ? d : null;
            }

            xml.Banco = GetElementValue(element, "Cod_Banco");
            xml.Agencia = GetElementValue(element, "Cod_Agencia");
            xml.Safe = GetElementValue(element, "Cod_Safe");
            xml.Seguridad = GetElementValue(element, "Cod_Seguridad_Planilla");
            xml.FechaTransmision = TryParseDate(GetElementValue(element, "Fe_Transmision"));
            xml.FechaRecaudacion = TryParseDate(GetElementValue(element, "Fe_Recaudacion"));
            xml.Rif = GetElementValue(element, "Rif_Contribuyente");
            xml.Forma = GetElementValue(element, "Cod_Forma");
            xml.Planilla = GetElementValue(element, "Num_Planilla");
            xml.Periodo = GetElementValue(element, "Periodo");
            xml.Aduana = GetElementValue(element, "Cod_Aduana");
            xml.Region = GetElementValue(element, "Cod_Region");
            xml.Electronico = GetElementValue(element, "Cancelado_Electronicamente");
            xml.Monto = TryParseDouble(GetElementValue(element, "Monto_Total_Transmision"));
            xml.Efectivo = TryParseDouble(GetElementValue(element, "Monto_Efectivo_Total"));
            xml.Cheque = TryParseDouble(GetElementValue(element, "Monto_Cheque_Total"));
            xml.Titulo = 0.0;
            xml.Cert = TryParseDouble(GetElementValue(element, "Monto_Cert_Total"));
            xml.Bono = TryParseDouble(GetElementValue(element, "Monto_Bonos_Export_Total"));
            xml.Dpn = TryParseDouble(GetElementValue(element, "Monto_Bonos_DPN_Total"));

            return xml;
        }

        private string GetElementValue(XElement parent, string elementName)
        {
            var element = parent.Element(elementName) ?? parent.Descendants(elementName).FirstOrDefault();
            return element?.Value.Trim() ?? "";
        }

        private TxtSeniat CrearTxt(XmlSeniat xml)
        {
            var txt = new TxtSeniat();

            // 1. Organismo
            string organismo = xml.Forma.Length >= 2 ? xml.Forma.Substring(0, 2) : "";
            txt.Organismo = organismo switch
            {
                "15" => "26",
                "99" => "00",
                "31" => "49",
                _ => organismo
            };

            // 2. Banco
            txt.Banco = xml.Banco.StartsWith("9") ? "9" + xml.Banco : xml.Banco;

            // 3. Agencia
            if (xml.Banco == "108" && xml.Agencia.StartsWith("4"))
                txt.Agencia = "2" + xml.Agencia;
            else
                txt.Agencia = "0" + xml.Agencia;

            // 4. Rif
            string rif = xml.Rif;
            string numeroRif = rif.Substring(1);
            txt.Rif = rif[0] switch
            {
                '1' => "V" + numeroRif,
                '2' => "E" + numeroRif,
                '3' => "J" + numeroRif,
                '4' => "P" + numeroRif,
                '5' => "G" + numeroRif,
                _ => "X" + numeroRif
            };

            // 5. Planilla
            txt.Planilla = xml.Planilla;

            // 6. Fecha Recaudación
            txt.FechaRecaudacion = xml.FechaRecaudacion;

            // 7. Tipo Transacción
            txt.TipoTransaccion = 2;

            // 8. Forma
            txt.Forma = xml.Forma;

            // 9. Efectivo + Cheque
            txt.Efectivo = (xml.Efectivo ?? 0) + (xml.Cheque ?? 0);

            // 10. Otros Pagos
            txt.OtrosPagos = (xml.Bono ?? 0) + (xml.Cert ?? 0) + (xml.Dpn ?? 0);

            // 11. Seguridad
            txt.Seguridad = xml.Seguridad;

            // 12. Safe
            txt.Safe = xml.Safe;

            // Campos adicionales
            /*txt.Estado = null;
            txt.Anho = 0;
            txt.LoteSeq = 0;
            txt.PlanSeq = 0;*/

            return txt;
        }



    public async Task<List<string>> GetXmlFiles()
    {
        var rootPath = _environment.ContentRootPath;
        var xmlFolderPath = Path.Combine(rootPath, "xml");

        if (!Directory.Exists(xmlFolderPath))
            return new List<string>();

        return Directory.GetFiles(xmlFolderPath, "*.xml")
            .Select(path =>
            {
                var fileInfo = new FileInfo(path);
                var name = fileInfo.Name;
                var date = fileInfo.LastWriteTime.ToString("dd/MM/yyyy");
                var size = BytesToSizeString(fileInfo.Length);
                return $"{name} {date} {size}";
            })
            .OrderBy(x => x)
            .ToList();
    }

    private string BytesToSizeString(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }
        return $"{len:F2}".Replace('.', ',') + " " + sizes[order];
    }
    

}