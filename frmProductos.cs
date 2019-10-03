using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductosConDB
{
    public partial class frmProductos : Form
    {
        // Creo una variable para el tamaño del arreglo que guardará los productos.
        const int tam = 99;
        // Creo un contador.
        int cont;
        // Creo una bandera para saber si al grabar estoy creando un artículo o editando uno.
        // FALSE: UPDATE - TRUE: INSERT // Default: False.
        bool nuevo;
        // Instancio mi clase de acceso a datos con la cadena de conexion como parámetro.
        AccesoDatos aDato = new AccesoDatos(@"Data Source=ALEXIS;Initial Catalog=Productos;Integrated Security=True");
        // Instancio mi clase de productos como array.
        Producto[] ap = new Producto[tam];
        public frmProductos()
        {
            InitializeComponent();
            // Inicializo el contador.
            cont = 0;
            // Inicializo el array con todos sus valores en null.
            for (int i = 0; i < tam; i++)
            {
                ap[i] = null;
            }
            // Inicializo la bandera con su valor por defecto (update).
            nuevo = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Utilizo mi método para cargar Comboboxes.
            this.CargarCombo(cbxMarca, "Marcas");
            // Pongo por inicio el Combobox sin selección.
            this.cbxMarca.SelectedIndex = -1;
            // Utilizo mi método para cargar ListViews.
            this.CargarLista("Productos");
            // Deshabilito el campo código ya que el PK de mi base de datos es Identity.
            this.txtCodigo.Enabled = false;
            // Utilizo mi método habilitar para inicializar el formulario con los campos deshabilitados.
            this.Habilitar(false);
        }

        // Creo un método que me sirva para habilitar o dehabilitar los campos,
        // usando como parámetro un booleano que me dirá si lo que quiero es activar o desactivar.
        private void Habilitar(bool x) {
            // Campos y botones a habilitar.
            this.txtDetalle.Enabled = x;
            this.cbxMarca.Enabled = x;
            this.rbtNetbook.Enabled = x;
            this.rbtNotebook.Enabled = x;
            this.txtPrecio.Enabled = x;
            this.dtpFecha.Enabled = x;
            this.btnCancelar.Enabled = x;
            this.btnGuardar.Enabled = x;

            // Campos y botones a deshabilitar (negando el valor del parámetro).
            this.btnNuevo.Enabled = !x;
            this.btnEditar.Enabled = !x;
            this.btnBorrar.Enabled = !x;
            this.btnSalir.Enabled = !x;
            this.lstProductos.Enabled = !x;
        }

        // Creo un método para limpiar todos los campos y volverlos a su valor por defecto.
        private void Limpiar() {
            txtCodigo.Clear();
            txtDetalle.Clear();
            txtPrecio.Clear();
            rbtNetbook.Checked = false;
            rbtNotebook.Checked = false;
            cbxMarca.SelectedIndex = -1;
            dtpFecha.Value = DateTime.Today;
        }

        // Creo un método para cargar los comboboxes del formulario en el cual le doy como
        // parámetro en nombre del combo a cargar y el nombre de la tabla de donde salen los datos.
        private void CargarCombo(ComboBox combo, string nombreTabla) {
            // Instancio la clase DataTable de System.Data.
            DataTable tabla = new DataTable();
            // En mi objeto tabla guardo el resultado de la consulta de mi tabla.
            tabla = aDato.consultarTabla(nombreTabla);
            // Le digo al combobox cual será la fuente de sus datos.
            combo.DataSource = tabla;
            // Le digo que los VALORES del combo se regirán por lo que esté en la columna "0" (PK).
            combo.ValueMember = tabla.Columns[0].ColumnName;
            // Luego le digo que lo que me tiene que mostrar está en la columna "1" (detalles).
            combo.DisplayMember = tabla.Columns[1].ColumnName;
            // Finalmente cancelo la posibilidad de que el combo pueda ser editado.
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // Creo un método para cargar las listas y como parámetro le paso el nombre de la tabla
        // de donde deberá sacar los datos.
        private void CargarLista(string nombreTabla) {
            // Creo un contador local
            cont = 0;
            // Primero limpio los valores que ya se encuentren en la lista.
            lstProductos.Items.Clear();
            // De mi objeto de conexion le digo que quiero leer la tabla que le pase por parámetro.
            aDato.LeerTabla(nombreTabla);
            // Y que mientras haya registros los lea (al ser DataReader me lee fila por fila).
            while (aDato.pLector.Read())
            {
                // Entonces instancio un nuevo producto local "p".
                Producto p = new Producto();
                // Y hago el mapeado del objeto "p" poniendo en cada campo el valor que me devuelve el registro traído.
                p.pCodigo = aDato.pLector.GetInt32(0);
                p.pDetalle = aDato.pLector.GetString(1);
                p.pTipo = aDato.pLector.GetInt32(2);
                p.pMarca = aDato.pLector[3].ToString(); // También puedo hacerlo así o con el nombre de la columna. 
                p.pPrecio = Convert.ToDouble(aDato.pLector[4]);
                p.pFecha = aDato.pLector.GetDateTime(5);
                // finalmente le doy a mi array en su posición actual los datos que guardé en mi objeto local "p".
                ap[cont] = p;
                // Y le digo al contador que avance una posición en el arreglo.
                cont++;
            }
            // Cuando ya no queden más registros freno el lector.
            aDato.pLector.Close();
            // Y desconecto la DB manualmente.
            aDato.Desconectar();
            // Entonces recorro mi arreglo y agrego los valores que contenga en los items de la lista.
            for (int i = 0; i < cont; i++)
            {
                lstProductos.Items.Add(ap[i].toString());
            }
            // Finalmente le digo a la lista que me seleccione por defecto el primer valor.
            lstProductos.SelectedIndex = 0;
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            this.nuevo = true; // Le digo a la bandera que voy a hacer un INSERT
            this.Habilitar(true); // Habilito los campos y deshabilito los botones ABM
            this.Limpiar(); // Limpio todos los campos del formulario
            this.txtDetalle.Focus(); // Y le doy foco en el primero.
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            this.Habilitar(true); // Habilito los campos y deshabilito los botones ABM
            this.txtDetalle.Focus(); // Le doy foco al primer campo.
        }

        private void BtnBorrar_Click(object sender, EventArgs e)
        {
            // Muestro un cuadro de texto consultando para confirmar el borrado.
            if (MessageBox.Show("Está Seguro de eliminar este elemento?", // Texto de la pregunta.
                                "ELIMINANDO", // Texto de la ventana.
                                MessageBoxButtons.YesNo, // Botones Confirmar/Cancelar.
                                MessageBoxIcon.Warning, // Símbolo advertencia.
                                MessageBoxDefaultButton.Button2) // Por defecto, focus en cancelar.
                                == DialogResult.Yes) // Si eso me da OK, prosigo:
            {
                // Creo una variable con el string de la consulta.
                string consultaSQL = "DELETE FROM Productos WHERE codigo =" + 
                                        ap[lstProductos.SelectedIndex].pCodigo; // El PK que quiero eliminar
                                        // lo saco del pCodigo del arreglo en la posicion seleccionada.
                // Mando a la DB el Query.
                aDato.ActualizarDB(consultaSQL);
                // Y vuelvo a cargar la lista.
                this.CargarLista("Productos");
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            // Aplico un condicional con mi función de validar.
            if (Validar())
            {
                // Creo una variable para almacenar mi Query.
                string consultaSQL;
                // Vuelvo el formulario a su posición inicial (todo deshabilitado menos ABM).
                this.Habilitar(false);
                // Instancio mi clase Producto creando un objeto "p" local.
                Producto p = new Producto();
                
                // Ahora confirmo con un condicional si se trata de un SELECT o de un UPDATE.
                if (this.nuevo)
                {
                    // Mapeo y le doy a los valores de mi objeto todo los datos que obtenga del formulario.
                    p.pDetalle = txtDetalle.Text;
                    if (rbtNotebook.Checked)
                    {
                        p.pTipo = 1;
                    }
                    else
                    {
                        p.pTipo = 2;
                    }
                    p.pMarca = Convert.ToString(cbxMarca.SelectedValue);
                    p.pPrecio = Convert.ToDouble(txtPrecio.Text);
                    p.pFecha = dtpFecha.Value;
                    // Para comprobar si el PK ya existe o no.
                    /*if (!existe(p.pCodigo))
                    {
                        ||| acá iría el insert |||
                    }
                    else
                    {
                        MessageBox.Show("Ya hay un producto con ese código registrado.");
                    }*/

                    // Si me dice que es un producto nuevo, hago el INSERT
                    // Construyo el Query en mi variable.
                    consultaSQL = "INSERT INTO Productos (detalle, tipo, id_marca, precio, fecha) VALUES('"
                                       + p.pDetalle + "',"
                                       + p.pTipo + ","
                                       + p.pMarca + ","
                                       + p.pPrecio + ",'"
                                       + p.pFecha.ToShortDateString() + "')";
                    // Ejecuto el Query en la DB.
                    aDato.ActualizarDB(consultaSQL);
                    // Y recargo nuevamente la lista.
                    this.CargarLista("Productos");
                }
                else
                {
                    // Mapeo y le doy a los valores de mi objeto todo los datos que obtenga del formulario.
                    p.pCodigo = Convert.ToInt32(txtCodigo.Text);
                    p.pDetalle = txtDetalle.Text;
                    if (rbtNotebook.Checked)
                    {
                        p.pTipo = 1;
                    }
                    else
                    {
                        p.pTipo = 2;
                    }
                    p.pMarca = Convert.ToString(cbxMarca.SelectedValue);
                    p.pPrecio = Convert.ToDouble(txtPrecio.Text);
                    p.pFecha = dtpFecha.Value;
                    // Si la bandera me dice que estoy modificando, hago un UPDATE:
                    // Construyo el Query en la variable.
                    consultaSQL = "UPDATE Productos SET detalle = '" + p.pDetalle +
                                        "', tipo = " + p.pTipo +
                                        ", id_marca = " + p.pMarca +
                                        ", precio =" + p.pPrecio +
                                        ", fecha = '" + p.pFecha.ToShortDateString() +
                                        "' WHERE codigo =" + p.pCodigo;
                    // Ejecuto el Query en la DB.
                    aDato.ActualizarDB(consultaSQL);
                    // Y actualizo la lista.
                    this.CargarLista("Productos");
                }
            }
            // Una vez terminado vuelvo a poner mi bandera en su posición original.
            this.nuevo = false;
            // Y vuelvo a deshabilitar todos los campos.
            this.Habilitar(false);
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            // Si cancelo, primero vuelvo la bandera a su valor por defecto.
            this.nuevo = false;
            // Deshabilito de nuevo todos los campos.
            this.Habilitar(false);
            // Limpio todo el formulario.
            this.Limpiar();
            // Y actualizo los campos según la selección de la lista.
            this.ActualizarCampos(lstProductos.SelectedIndex);
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            // No necesito hacer la verificación acá ya que al hacerla en el evento CERRANDO
            // afecta a esto también.
            this.Close();
        }

        private void LstProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Al cambiar el index seleccionado en la lista actualizo todos los campos con sus valores.
            this.ActualizarCampos(lstProductos.SelectedIndex);
        }

        // Creo este método para actualizar los valores de los campos al moverme de indice en la lista.
        private void ActualizarCampos(int posicion) {
            // Mapeo el arreglo en la posicion que traiga de los parámetros (en este caso el index de la lista)
            txtCodigo.Text = ap[posicion].pCodigo.ToString();
            txtDetalle.Text = ap[posicion].pDetalle;
            txtPrecio.Text = ap[posicion].pPrecio.ToString();
            if (ap[posicion].pTipo == 1) // Si el tipo es 1, sleecciono Notebook
            {
                rbtNotebook.Checked = true;
                rbtNetbook.Checked = false;
            } else {   // Si no, sleecciono Netbook
                rbtNotebook.Checked = false;
                rbtNetbook.Checked = true;
            }
            dtpFecha.Value = ap[posicion].pFecha;
            cbxMarca.SelectedValue = ap[posicion].pMarca; // Uso el value porque es el valor que yo tengo (fk de la marca)
        }

        // Método por si no tuviera mi PK autoincremental
        /*private bool existe(int pk)
        {
            for (int i = 0; i < cont; i++)
            {
                if (ap[i].pCodigo == pk)
                {
                    return true;
                }
            }
            return false;
        }*/

        // Método para validar todo previo a enviar el formulario.
        private bool Validar() {
            if (string.IsNullOrEmpty(txtDetalle.Text))
            {
                MessageBox.Show("Debe ingresar un detalle.");
                txtDetalle.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtPrecio.Text))
            {
                MessageBox.Show("Debe ingresar un precio.");
                txtPrecio.Focus();
                return false;
            } else {
                try
                {
                    Double.Parse(txtPrecio.Text);
                }
                catch
                {
                    MessageBox.Show("Debe ingresar solamente números.");
                    txtPrecio.Focus();
                    return false;
                }
            }
            if (cbxMarca.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar una marca.");
                cbxMarca.Focus();
                return false;
            }
            if (!rbtNotebook.Checked && !rbtNetbook.Checked)
            {
                MessageBox.Show("Debe seleccionar un tipo.");
                rbtNotebook.Focus();
                return false;
            }
            if (dtpFecha.Value > DateTime.Now)
            {
                MessageBox.Show("La fecha no puede ser posterior al día de hoy.");
                dtpFecha.Focus();
                return false;
            }
            return true;
        }

        // Modifico el evento CERRANDO por defecto.
        private void FrmProductos_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; // Primero le digo que no cierre.
            // Luego le consulto si está seguro de cerrar.
            if (MessageBox.Show("¿Está seguro de querer salir?",
                            "SALIENDO",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button2)
                            == DialogResult.Yes)
            {
                // Si lo está, cierro.
                e.Cancel = false;
            }
        }
    }
}
