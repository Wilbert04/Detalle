using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TelefonosConDetalle.BLL;
using TelefonosConDetalle.Entidades;

namespace TelefonosConDetalle.UI
{
    /// <summary>
    /// Interaction logic for rPersonas.xaml
    /// </summary>
    public partial class rPersonas : Window
    {
        public List<TelefonosDetalle> Detalles { get; set; }
        public rPersonas()
        {
            InitializeComponent();
            this.Detalles = new List<TelefonosDetalle>();
        }

        private void LimpiarCampos()
        {
            nombreTextBox.Text = string.Empty;
            direccionTextBox.Text = string.Empty;
            cedulaTextBox.Text = string.Empty;
            fechanacDatePicker.SelectedDate = DateTime.Now;
            IdTextBox.Text = "0";

            this.Detalles = new List<TelefonosDetalle>();
            CargarGrid();
        }

        private Personas LlenaClase()
        {
            Personas personas = new Personas();

            personas.PersonaId = Convert.ToInt32(IdTextBox.Text);
            personas.Nombre = nombreTextBox.Text;
            personas.Direccion = direccionTextBox.Text;
            personas.Cedula = cedulaTextBox.Text;
            personas.FechaNacimiento = fechanacDatePicker.DisplayDate;

            personas.Telefonos = this.Detalles; 

            return personas;
        }

        private void LlenaCampo(Personas personas)
        {
            IdTextBox.Text = Convert.ToString(personas.PersonaId);
            nombreTextBox.Text = personas.Nombre;
            direccionTextBox.Text = personas.Direccion;
            cedulaTextBox.Text = personas.Cedula;
            fechanacDatePicker.SelectedDate = personas.FechaNacimiento;

            this.Detalles = personas.Telefonos;
            CargarGrid();
        }

        private bool Validar()
        {
            bool paso = true;

            if (string.IsNullOrWhiteSpace(nombreTextBox.Text))
            {
                MessageBox.Show("No puedo haber campos vacios!!");
                paso = false;
            }

            if (string.IsNullOrWhiteSpace(direccionTextBox.Text))
            {
                MessageBox.Show("No puedo haber campos vacios!!");
                paso = false;
            }

            if (string.IsNullOrWhiteSpace(cedulaTextBox.Text))
            {
                MessageBox.Show("No puedo haber campos vacios!!");
                paso = false;
            }

            if (this.Detalles.Count == 0)
            {
                MessageBox.Show("Debe Agregar un Telefono!!");
                paso = false;
            }

            return paso;
        }

        private bool ExisteEnLaBaseDatos()
        {
            Personas personas = PersonaBLL.Buscar((int)Convert.ToInt32(IdTextBox.Text));
            return (personas != null);
        }

        private void CargarGrid()
        {
            DataGrid.ItemsSource = null;
            DataGrid.ItemsSource = this.Detalles;
        }

        private void nuevoButton_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }

        private void guardarButton_Click(object sender, RoutedEventArgs e)
        {
            Personas personas;
            bool paso = false;

            if (!Validar())
                return;

            personas = LlenaClase();


            if (IdTextBox.Text == "0")
                paso = PersonaBLL.Guardar(personas);

            else
            {
                if (!ExisteEnLaBaseDatos())
                {
                    MessageBox.Show("Personas No Existe!!");
                }
                MessageBox.Show("Persona Modificada!!");
                paso = PersonaBLL.Modificar(personas);
            }

            if (paso)
            {
                MessageBox.Show("¡¡Guardado!!");
            }
            else
            {
                MessageBox.Show("No se Guardo!!");
            }

        }

        private void buscarButton_Click(object sender, RoutedEventArgs e)
        {
            int id;
            int.TryParse(IdTextBox.Text, out id);
            Personas personas = new Personas();

            personas = PersonaBLL.Buscar(id);

            if (personas != null)
            {
                MessageBox.Show("Persona Encontrada!!");
                LlenaCampo(personas);
            }
            else
            {
                MessageBox.Show("Persona No Encontrada!!");
            }


        }

        private void EliminarButton__Click(object sender, RoutedEventArgs e)
        {
            int id;
            int.TryParse(IdTextBox.Text, out id);


            if (PersonaBLL.Eliminar(id))
            {
                MessageBox.Show("Eliminado con exito!!");
            }
            else
            {
                MessageBox.Show("No se pudo Eliminar!!");
            }
        }

        private void AgregarButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectedItem != null)
                this.Detalles = (List<TelefonosDetalle>)DataGrid.ItemsSource;


            this.Detalles.Add(
                new TelefonosDetalle(
                    id: 0,
                    personaId: Convert.ToInt32(IdTextBox.Text),
                    tipoTelefono: tipotelefonoTextBox.Text,
                    telefono: telefonoTextBox.Text

                    ));

            CargarGrid();
            telefonoTextBox.Clear();
            tipotelefonoTextBox.Clear();
        }

        private void removerButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid.Columns.Count > 0 && DataGrid.SelectedCells != null)

                Detalles.RemoveAt(DataGrid.SelectedIndex);

            CargarGrid();
        }

    }
}
