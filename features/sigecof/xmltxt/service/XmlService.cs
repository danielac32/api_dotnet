
//using System.Net.Mime;
using System.Globalization;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.AspNetCore.Hosting;
using backend_ont_2.Xmltxt.DataClass;
using backend_ont_2.OracleDbProject;


namespace backend_ont_2.Xmltxt.Service;

public class XmlService
{

    private readonly OracleDb _oracleDb;
    private readonly IWebHostEnvironment _environment;
    public XmlService(OracleDb oracleDb,IWebHostEnvironment environment)
    {
        _oracleDb = oracleDb;
        _environment = environment;
    }


    public string GetXmlUploadPath()
    {
        // Carpeta donde se guardarán los XML: /assets/xml
        return Path.Combine(_environment.ContentRootPath, "xml");
    }




    public async Task<List<FileUploadResponse>> UploadXmlFilesList(FileUploadRequestList request)
    {
        if (request?.Files == null || !request.Files.Any())
            throw new ArgumentException("No se proporcionaron archivos");

        var responses = new List<FileUploadResponse>();
        var uploadPath = GetXmlUploadPath();
        Directory.CreateDirectory(uploadPath);

        foreach (var file in request.Files)
        {
            if (string.IsNullOrWhiteSpace(file.Filename))
                throw new ArgumentException("Nombre de archivo no proporcionado");

            if (string.IsNullOrWhiteSpace(file.Content))
                throw new ArgumentException("Contenido Base64 no proporcionado");

            var extension = Path.GetExtension(file.Filename).TrimStart('.').ToLowerInvariant();
            if (extension != "xml")
                throw new ArgumentException($"Solo se permiten archivos XML (.xml). Archivo: {file.Filename}");

            byte[] fileBytes;
            try
            {
                fileBytes = Convert.FromBase64String(file.Content);
            }
            catch (FormatException)
            {
                throw new ArgumentException($"Formato Base64 inválido en el archivo: {file.Filename}");
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.Filename)}";
            var filePath = Path.Combine(uploadPath, uniqueFileName);

            await File.WriteAllBytesAsync(filePath, fileBytes);

            responses.Add(new FileUploadResponse
            {
                Success = true,
                Message = "Archivo XML subido correctamente",
                FileName = uniqueFileName,
                OriginalName = file.Filename,
                FileSize = fileBytes.Length
            });
        }

        return responses;
    }


    public async Task<FileUploadResponse> UploadXmlFileAsync(FileUploadRequest request)
    {
        if (request?.File == null)
            throw new ArgumentException("Datos de archivo no proporcionados");

        if (string.IsNullOrWhiteSpace(request.File.Filename))
            throw new ArgumentException("Nombre de archivo no proporcionado");

        if (string.IsNullOrWhiteSpace(request.File.Content))
            throw new ArgumentException("Contenido Base64 no proporcionado");

        var extension = Path.GetExtension(request.File.Filename).TrimStart('.').ToLowerInvariant();
        if (extension != "xml")
            throw new ArgumentException("Solo se permiten archivos XML (.xml)");

        byte[] fileBytes;
        try
        {
            fileBytes = Convert.FromBase64String(request.File.Content);
        }
        catch (FormatException)
        {
            throw new ArgumentException("Formato Base64 inválido");
        }

        var uploadPath = GetXmlUploadPath();
        Directory.CreateDirectory(uploadPath);

        var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(request.File.Filename)}";
        var filePath = Path.Combine(uploadPath, uniqueFileName);

        await File.WriteAllBytesAsync(filePath, fileBytes);

        return new FileUploadResponse
        {
            Success = true,
            Message = "Archivo XML subido correctamente",
            FileName = uniqueFileName,
            OriginalName = request.File.Filename,
            FileSize = fileBytes.Length
        };
    }

    public async Task<FileUploadResponse> Remove(string name)
    {
        var filePath = Path.Combine(GetXmlUploadPath(), name);

        if (!File.Exists(filePath))
            throw new ArgumentException("Archivo no encontrado");

        File.Delete(filePath);

        return new FileUploadResponse
        {
            Success = true,
            Message = "xml eliminado correctamente"
        };
    }

    public async Task<Dictionary<string, object>> ProcessXmlFiles()
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

        Console.WriteLine("\n=== RESUMEN FINAL ===");
        Console.WriteLine($"Total planillas procesadas: {totalPlanillas}");
        Console.WriteLine($"Total errores: {totalErrores}");
        Console.WriteLine("Resultados por archivo:");

        foreach (var r in resultados)
        {
            Console.WriteLine($"  - {r["archivo"]} | Planillas: {r["planillas"]}, Errores: {r["errores"]}");
        }

        return new Dictionary<string, object>
        {
            ["resultados"] = resultados,
            ["total_planillas"] = totalPlanillas,
            ["total_errores"] = totalErrores
        };
        // Mostrar resumen final (como respuesta JSON en Java)

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

                    if (new[] { "999", "000" }.Contains(xml.Banco) ||
                        new[] { "79084", "99084", "00084" }.Contains(xml.Forma))
                    {
                        continue;
                    }

                    var txt = CrearTxt(xml);
                    Console.WriteLine(txt.ToString());
                    
                    // 🔼 USAR AWAIT CORRECTAMENTE
                    int result = await _oracleDb.InsertTxtSeniatAsync(txt);
                    
                    if (result > 0)
                    {
                        Console.WriteLine($"✅ Planilla {txt.Planilla} insertada correctamente");
                        planillasProcesadas++;
                    }
                    else
                    {
                        Console.WriteLine($"⚠️ No se pudo insertar la planilla {txt.Planilla}");
                        errores++;
                    }
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
    string numero_rif = rif.Substring(1);

    if (rif.StartsWith("1"))
        rif = "V" + numero_rif;
    else if (rif.StartsWith("2"))
        rif = "E" + numero_rif;
    else if (rif.StartsWith("3"))
        rif = "J" + numero_rif;
    else if (rif.StartsWith("4"))
        rif = "P" + numero_rif;
    else if (rif.StartsWith("5"))
        rif = "G" + numero_rif;

    txt.Rif = rif; // ← asignación al final, como en Python


        /*
        txt.Rif = rif[0] switch
        {
            '1' => "V" + numeroRif,
            '2' => "E" + numeroRif,
            '3' => "J" + numeroRif,
            '4' => "P" + numeroRif,
            '5' => "G" + numeroRif,
            _ => "X" + numeroRif
        };*/

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