using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AccesoDatos.cad;
using AccesoDatos.Entidades;

namespace EjercicioPractico
{
    public partial class frmABC : Form
    {
        static DatosConexion datosConexion = new DatosConexion("MaestroMuebles", "10.44.172.83", "sysprogsmuebles", "0017fbf754d74de75f64e332e8a49aca");
        //static DatosConexion datosConexion = new DatosConexion("tienda.0204", "10.28.114.102", "sysprogsbm", "679ca9f6b15be2748009ab6c6f42c29d");
        static OdbcDataAccessLayer dbOdbc = new OdbcDataAccessLayer(datosConexion);

        public frmABC()
        {
            InitializeComponent();
        }

        private void frmABC_Load(object sender, EventArgs e)
        {
            //dbOdbc.ServidorPostgreSql();
            dbOdbc.ServidorSqlServer();

            if (!consultarClientes())
            {
                MessageBox.Show("Ocurrio un error al consultar la información, se cerrará la opción", "ABC", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void dgvClientes_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            string sMensaje = string.Format("Error: {0}", e.Exception.Message);

            MessageBox.Show(sMensaje, "ABC", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            accionGrid(e.ColumnIndex);
        }

        private void dgvClientes_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            accionGrid(e.ColumnIndex);
        }

        private void dgvClientes_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox txt = e.Control as TextBox;

            if (txt != null)
            {
                txt.CharacterCasing = CharacterCasing.Upper;
            }
        }

        private void frmABC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool consultarClientes()
        {
            bool bRegresa = false;

            IDataReader reader;
            DataSet data = new DataSet();
            DataTable dt = new DataTable("Clientes");

            try
            {
                reader = dbOdbc.executeDataReader("SELECT num_cliente, des_nombre, des_apellidopaterno, des_apellidomaterno, des_direccion FROM cat_clientes ORDER BY num_cliente ");

                data.Tables.Add(dt);
                data.Load(reader, LoadOption.PreserveChanges, data.Tables[0]);

                this.dgvClientes.DataSource = null;
                this.dgvClientes.DataSource = data.Tables[0];
                this.dgvClientes.Refresh();

                dgvClientes.Columns["num_cliente"].Visible = false;
                ((DataGridViewTextBoxColumn)dgvClientes.Columns["des_nombre"]).MaxInputLength = 20;
                ((DataGridViewTextBoxColumn)dgvClientes.Columns["des_apellidopaterno"]).MaxInputLength = 20;
                ((DataGridViewTextBoxColumn)dgvClientes.Columns["des_apellidomaterno"]).MaxInputLength = 20;
                ((DataGridViewTextBoxColumn)dgvClientes.Columns["des_direccion"]).MaxInputLength = 100;

                ((DataGridViewTextBoxColumn)dgvClientes.Columns["des_nombre"]).SortMode = DataGridViewColumnSortMode.NotSortable;
                ((DataGridViewTextBoxColumn)dgvClientes.Columns["des_apellidopaterno"]).SortMode = DataGridViewColumnSortMode.NotSortable;
                ((DataGridViewTextBoxColumn)dgvClientes.Columns["des_apellidomaterno"]).SortMode = DataGridViewColumnSortMode.NotSortable;
                ((DataGridViewTextBoxColumn)dgvClientes.Columns["des_direccion"]).SortMode = DataGridViewColumnSortMode.NotSortable;

                bRegresa = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ABC", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return bRegresa;
        }

        private void accionGrid(int iColumna)
        {
            bool bRegresa = false;

            if (iColumna == dgvClientes.Columns["update"].Index)
            {
                bRegresa = actualizarCliente();                
            }
            else if (iColumna == dgvClientes.Columns["delete"].Index)
            {
                if (validarVacios())
                {
                    bRegresa = eliminarCliente();
                }
            }
            else if (iColumna == dgvClientes.Columns["insert"].Index)
            {
                if (validarVacios())
                {
                    if (dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["num_cliente"].Value == null)
                    {
                        bRegresa = insertarCliente();
                    }
                    else
                    {
                        MessageBox.Show("Debe de ser un registro nuevo para insertarlo", "ABC", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            if (bRegresa)
            {
                MessageBox.Show("Accion realizada correctamente", "ABC", MessageBoxButtons.OK, MessageBoxIcon.Information);

                consultarClientes();
            }
        }

        private bool validarVacios()
        {
            bool bRegresa = false;

            try
            {
                if ((dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["des_nombre"].Value != null && dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["des_nombre"].Value.ToString() != string.Empty) &&
                    (dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["des_apellidopaterno"].Value != null && dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["des_apellidopaterno"].Value.ToString() != string.Empty) &&
                    (dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["des_apellidomaterno"].Value != null && dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["des_apellidomaterno"].Value.ToString() != string.Empty) &&
                    (dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["des_direccion"].Value != null && dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["des_direccion"].Value.ToString() != string.Empty))
                {
                    bRegresa = true;
                }
                else
                {
                    MessageBox.Show("Favor de capturar la información completa", "ABC", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ABC", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            

            return bRegresa;
        }

        private bool insertarCliente()
        {
            bool bRegresa = false;
            string sQuery = string.Empty;

            try
            {

                sQuery = string.Format(" INSERT INTO cat_clientes (des_nombre, des_apellidopaterno, des_apellidomaterno, des_direccion)  VALUES ('{0}', '{1}', '{2}', '{3}') ",
                                        dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["des_nombre"].Value.ToString(),
                                        dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["des_apellidopaterno"].Value.ToString(),
                                        dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["des_apellidomaterno"].Value.ToString(),
                                        dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["des_direccion"].Value.ToString());

                dbOdbc.executeQuery(sQuery);

                bRegresa = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ABC", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return bRegresa;
        }

        private bool actualizarCliente()
        {
            bool bRegresa = false;
            string sQuery = string.Empty;

            try
            {

                sQuery = string.Format(" UPDATE cat_clientes SET des_nombre = '{0}', des_apellidopaterno = '{1}', des_apellidomaterno = '{2}', des_direccion = '{3}' WHERE num_cliente = '{4}' ",
                                        dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["des_nombre"].Value.ToString(),
                                        dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["des_apellidopaterno"].Value.ToString(),
                                        dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["des_apellidomaterno"].Value.ToString(),
                                        dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["des_direccion"].Value.ToString(),
                                        dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["num_cliente"].Value.ToString());

                dbOdbc.executeQuery(sQuery);

                bRegresa = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ABC", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return bRegresa;
        }

        private bool eliminarCliente()
        {
            bool bRegresa = false;
            string sQuery = string.Empty;

            try
            {

                sQuery = string.Format(" DELETE FROM cat_clientes WHERE num_cliente = '{0}' ",
                                        dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells["num_cliente"].Value.ToString());

                dbOdbc.executeQuery(sQuery);

                bRegresa = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ABC", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return bRegresa;
        }                
    }
}
