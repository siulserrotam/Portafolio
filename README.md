# Portafolio
Developer fullstak Angular y C# con .Net Core
----------------------------------Web.config------------------------------------------------

<configuration>
  <connectionStrings>
    <add name="OracleConexion" 
         connectionString="User Id=LUISTO;Password=1234;Data Source=//localhost:1521/XE;" 
         providerName="Oracle.ManagedDataAccess.Client" />
  </connectionStrings>
</configuration>
------------------------------Default.aspx.cs-----------------------------------------------
using System;
using System.Configuration; 
using System.Data;
using System.Linq; 
using Oracle.ManagedDataAccess.Client; 

namespace PruebaTecnicaOracle 
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly string connectionString = 
            ConfigurationManager.ConnectionStrings["OracleConexion"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrilla(); 
            }
        }

        private void CargarGrilla()
        {
            // Consulta SQL con LISTAGG para concatenar teléfonos
            string sql = @"
                SELECT p.Id, p.Nombre, p.Apellido,
                    LISTAGG(t.Numero || ' (' || CASE WHEN t.Tipo = 0 THEN 'F' ELSE 'C' END || ')', ' - ') 
                    WITHIN GROUP (ORDER BY t.Tipo) AS Telefonos
                FROM Personas p
                LEFT JOIN Telefonos t ON p.Id = t.idPersona
                GROUP BY p.Id, p.Nombre, p.Apellido
                ORDER BY p.Id DESC"; 

            using (OracleConnection con = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(sql, con))
                {
                    try
                    {
                        con.Open();
                        DataTable dt = new DataTable();
                        new OracleDataAdapter(cmd).Fill(dt);
                        
                        GridViewRegistros.DataSource = dt;
                        GridViewRegistros.DataBind();
                    }
                    catch (Exception ex)
                    {
                        litMensaje.Text = $"<span style='color: red;'>Error al cargar la grilla: {ex.Message}</span>";
                    }
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text) || string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                litMensaje.Text = "<span style='color: orange;'>Por favor, complete todos los campos.</span>";
                return;
            }
            
            int tipoTelefono = 1; 

            using (OracleConnection con = new OracleConnection(connectionString))
            {
                con.Open();
                OracleTransaction transaction = con.BeginTransaction();

                try
                {
                    // 1. Inserción de Persona y obtención del ID con RETURNING INTO
                    string sqlPersona = "INSERT INTO Personas (Nombre, Apellido) VALUES (:p_Nombre, :p_Apellido) RETURNING Id INTO :p_IdPersona";
                    OracleParameter idPersonaParam = new OracleParameter("p_IdPersona", OracleDbType.Decimal, ParameterDirection.Output);
                    
                    using (OracleCommand cmdPersona = new OracleCommand(sqlPersona, con, transaction))
                    {
                        cmdPersona.Parameters.Add("p_Nombre", OracleDbType.Varchar2).Value = txtNombre.Text;
                        cmdPersona.Parameters.Add("p_Apellido", OracleDbType.Varchar2).Value = txtApellido.Text;
                        cmdPersona.Parameters.Add(idPersonaParam); 
                        cmdPersona.ExecuteNonQuery();

                        decimal idPersona = (decimal)idPersonaParam.Value;

                        // 2. Inserción de Teléfono
                        string sqlTelefono = "INSERT INTO Telefonos (Numero, Tipo, idPersona) VALUES (:p_Numero, :p_Tipo, :p_IdPersonaFK)";
                        
                        using (OracleCommand cmdTelefono = new OracleCommand(sqlTelefono, con, transaction))
                        {
                            string numeroLimpio = new string(txtTelefono.Text.Where(char.IsDigit).ToArray());
                            
                            // Mantenido como decimal/NUMBER para coincidir con la definición de tu DB
                            cmdTelefono.Parameters.Add("p_Numero", OracleDbType.Decimal).Value = decimal.Parse(numeroLimpio); 
                            cmdTelefono.Parameters.Add("p_Tipo", OracleDbType.Decimal).Value = tipoTelefono;
                            cmdTelefono.Parameters.Add("p_IdPersonaFK", OracleDbType.Decimal).Value = idPersona;
                            
                            cmdTelefono.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    litMensaje.Text = "<span style='color: green;'>Registro guardado exitosamente.</span>";
                    CargarGrilla();
                    LimpiarCampos();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    litMensaje.Text = $"<span style='color: red;'>Error al guardar: {ex.Message}</span>";
                }
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            litMensaje.Text = string.Empty; 
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtTelefono.Text = string.Empty;
        }
    }
}
---------------------------------Default.aspx-------------------------------------------------
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PruebaTecnicaOracle.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Prueba Técnica ASP.NET Oracle</title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Desarrollo de Aplicación Web</h1>

        <h2>1. Formulario de Ingreso</h2>

        <div>
            <asp:Label ID="lblNombre" runat="server" Text="Nombre:"></asp:Label>
            <asp:TextBox ID="txtNombre" runat="server"></asp:TextBox><br />
        </div>

        <div>
            <asp:Label ID="lblApellido" runat="server" Text="Apellido:"></asp:Label>
            <asp:TextBox ID="txtApellido" runat="server"></asp:TextBox><br />
        </div>

        <div>
            <asp:Label ID="lblTelefono" runat="server" Text="Teléfono:"></asp:Label>
            <asp:TextBox ID="txtTelefono" runat="server" MaxLength="15"></asp:TextBox><br />
        </div>

        <div style="margin-top: 10px;">
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" CausesValidation="false" />
        </div>

        <p><asp:Literal ID="litMensaje" runat="server" /></p>
        
        <hr />

        <h2>2. Grilla de Registros</h2>
        <asp:GridView 
            ID="GridViewRegistros" 
            runat="server" 
            AutoGenerateColumns="false"
            EmptyDataText="No hay registros.">
            <Columns>
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                <asp:BoundField DataField="Telefonos" HeaderText="Teléfonos" />
            </Columns>
        </asp:GridView>

    </form>
</body>
</html>
---------------------------------------------------------------------
-- TABLA PRINCIPAL: PERSONAS
CREATE TABLE Personas (
    Id NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY PRIMARY KEY,
    Nombre VARCHAR2(100) NOT NULL,
    Apellido VARCHAR2(100) NOT NULL
);

-- TABLA RELACIONADA: TELEFONOS
CREATE TABLE Telefonos (
    Id NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY PRIMARY KEY,
    Numero NUMBER(15) NOT NULL,
    Tipo NUMBER(1) NOT NULL, 
    idPersona NUMBER NOT NULL,
    CONSTRAINT FK_Personas
        FOREIGN KEY (idPersona)
        REFERENCES Personas(Id)
        ON DELETE CASCADE
);
