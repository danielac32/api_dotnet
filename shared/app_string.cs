public static class AppStrings
{
    // Roles de usuario
    public const string SuperAdmin = "SUPER_ADMIN";
    public const string DepartmentAdmin = "DEPARTMENT_ADMIN";
    public const string Editor = "EDITOR";
    public const string Viewer = "VIEWER";
    public const string Guest = "GUEST";
    public const string User = "USER";
    public const string Admin = "ADMIN";

    // Departamentos
    public const string DgAdministracion = "DIRECCIÓN GENERAL DE ADMINISTRACIÓN";
    public const string DgEgreso = "DIRECCIÓN GENERAL DE EGRESO";
    public const string DgIngreso = "DIRECCIÓN GENERAL DE INGRESO";
    public const string DgCuentaUnica = "DIRECCIÓN GENERAL DE CUENTA ÚNICA";
    public const string DgTecnologiaInformacion = "DIRECCIÓN GENERAL DE TECNOLOGÍA DE INFORMACIÓN";
    public const string DgPlanificacionAnalisisFinanciero = "DIRECCIÓN GENERAL DE PLANIFICACIÓN Y ANÁLISIS FINANCIERO";
    public const string DgRecursosHumanos = "DIRECCIÓN GENERAL DE RECURSOS HUMANOS";
    public const string DgInversionesYValores = "DIRECCIÓN GENERAL DE INVERSIONES Y VALORES";
    public const string DgConsultoriaJuridica = "DIRECCIÓN GENERAL DE CONSULTORÍA JURÍDICA";

    // Secciones para permisos
    public const string Carrusel = "CARRUSEL";
    public const string Organismos = "ORGANISMOS";
    public const string Gobernacion = "GOBERNACION";
    public const string Alcaldias = "ALCALDIAS";
    public const string ProgramacionFinanciera = "PROGRAMACION_FINANCIERA";
    public const string ResumenGestion = "RESUMEN_GESTION";
    public const string Noticias = "NOTICIAS";
    public const string Configuracion = "CONFIGURACION";

    // Tipos de valores para programación financiera
    public const string PresupuestoInicial = "PRESUPUESTO_INICIAL";
    public const string PresupuestoFinal = "PRESUPUESTO_FINAL";
    public const string GastoReal = "GASTO_REAL";

    // Cargos
    public const string Coordinador = "COORDINADOR";
    public const string DirectorGeneral = "DIRECTOR GENERAL";
    public const string DirectorLinea = "DIRECTOR DE LINEA";
    public const string Asistente = "ASISTENTE";
    public const string Analista = "ANALISTA";
    public const string Asesor = "ASESOR";
    public const string Consultor = "CONSULTOR";
    public const string Hp = "HP";
    public const string Otro = "OTRO";

    // Lista de secciones
    public static readonly List<string> Sections = new List<string>
    {
        Carrusel,
        /*Alcaldias,
        Organismos,
        Gobernacion,
        Noticias,*/
        ProgramacionFinanciera,
        ResumenGestion
        // Agrega más secciones según sea necesario
    };
}